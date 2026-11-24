using Application;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Asp.Versioning.Conventions;
using EsApi.Filters;
using EsApi.Middlewares;
using EsApi.Swagger;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Security;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Options;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Threading.RateLimiting;


var builder = WebApplication.CreateBuilder(args);

#region Sentry
builder.WebHost.UseSentry(options =>
{
    options.Dsn = builder.Configuration["Sentry:Dsn"];
    options.Environment = builder.Configuration["Sentry:Environment"]
                          ?? builder.Environment.EnvironmentName;

    options.TracesSampleRate = builder.Configuration
        .GetValue<double?>("Sentry:TracesSampleRate") ?? 0.0;

    options.SendDefaultPii = false;
    options.Debug = builder.Environment.IsDevelopment();
    options.SetBeforeSend(sentryEvent =>
    {
        sentryEvent.Request.QueryString = null;
        return sentryEvent;
    });
    options.SetBeforeSendTransaction(transaction =>
    {
        transaction.Request.QueryString = null;
        return transaction;
    });
});
#endregion

#region Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();
#endregion


#region Controllers
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilter>();
});
#endregion


#region ApiVersioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
})
.AddMvc(options =>
{
    options.Conventions.Add(new VersionByNamespaceConvention());
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddEndpointsApiExplorer();
#endregion

#region Swagger
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(options => options.EnableAnnotations());
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.RequestMethod |
                            HttpLoggingFields.RequestPath |
                            HttpLoggingFields.ResponseStatusCode |
                            HttpLoggingFields.Duration;
    options.CombineLogs = true;
    options.RequestHeaders.Add("X-Correlation-ID");
    options.ResponseHeaders.Add("Content-Type");
});
#endregion

#region DependencyInjection
builder.AddInfrastructure();
builder.Services.AddApplication();
#endregion

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("LoginPolicy", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            }));

    options.AddPolicy("DefaultPolicy", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.User?.Identity?.Name
                ?? httpContext.Connection.RemoteIpAddress?.ToString()
                ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 120,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            }));
});

var app = builder.Build();

var bootstrapPassword = app.Configuration["API_BOOTSTRAP_ADMIN_PASSWORD"];
if (!string.IsNullOrWhiteSpace(bootstrapPassword))
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    await DefaultAdminSeeder.SeedAsync(context, bootstrapPassword);
}

app.UseMiddleware<CorrelationIdMiddleware>();

app.UseSerilogRequestLogging();

app.UseMiddleware<UnhandledExceptionMiddleware>();

app.UseHttpLogging();

if (app.Environment.IsDevelopment())
{
    var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }

        options.DocumentTitle = "Operations API";
        options.RoutePrefix = string.Empty;
    });

}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers();

app.Run();

public partial class Program;
