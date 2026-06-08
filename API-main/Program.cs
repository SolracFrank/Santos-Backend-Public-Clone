using Application;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Asp.Versioning.Conventions;
using EsApi.Filters;
using EsApi.Middlewares;
using EsApi.Swagger;
using Infrastructure;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Options;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;


var builder = WebApplication.CreateBuilder(args);

#region Sentry
builder.WebHost.UseSentry(options =>
{
    options.Dsn = builder.Configuration["Sentry:Dsn"];
    options.Environment = builder.Configuration["Sentry:Environment"]
                          ?? builder.Environment.EnvironmentName;

    options.TracesSampleRate = builder.Configuration
        .GetValue<double?>("Sentry:TracesSampleRate") ?? 0.0;

    options.SendDefaultPii = builder.Configuration
        .GetValue<bool?>("Sentry:SendDefaultPii") ?? false;

    options.Debug = true;
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


var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();
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
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }

        options.DocumentTitle = "Swagger - Jime-store";
        options.RoutePrefix = string.Empty;
    });

    app.MapGet("/", context =>
    {
        context.Response.Redirect("/swagger");
        return Task.CompletedTask;
    });

    app.MapGet("/test-sentry", () =>
    {
        throw new Exception("Sentry test exception");
    });
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSerilogRequestLogging();

app.MapControllers();

app.Run();
