namespace Infrastructure;

using Domain.Common.Audit;
using Domain.Common.Security;
using Domain.Models.ConfigurationModels;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Security;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
public static class DependencyInjection
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
         ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString));
        });

        // Brevo Configuration
        builder.Services.Configure<MicrosoftGraphEmailConfiguration>(
        builder.Configuration.GetSection("MicrosoftGraphEmail")
        );

        builder.Services.AddOptions<EmailAttachmentOptions>()
            .Bind(builder.Configuration.GetSection(EmailAttachmentOptions.SectionName))
            .Validate(options => options.MaxFileSizeMb > 0, "EmailAttachments:MaxFileSizeMb must be greater than zero.")
            .Validate(options => options.MaxFileCount > 0, "EmailAttachments:MaxFileCount must be greater than zero.")
            .Validate(options => options.AllowedExtensions is { Length: > 0 }, "EmailAttachments:AllowedExtensions cannot be empty.")
            .Validate(
                options => options.AllowedExtensions is not null && options.AllowedExtensions.All(EmailAttachmentSecurity.IsSupportedExtension),
                "EmailAttachments:AllowedExtensions contains an unsupported extension.")
            .ValidateOnStart();

        // Configuration
        builder.Services.Configure<JwtConfiguration>(
            builder.Configuration.GetSection(JwtConfiguration.SectionName));

        var jwtConfiguration = builder.Configuration
            .GetSection(JwtConfiguration.SectionName)
            .Get<JwtConfiguration>();

        if (jwtConfiguration is null)
        {
            throw new InvalidOperationException("JWT configuration was not found.");
        }

        if (string.IsNullOrWhiteSpace(jwtConfiguration.SecretKey))
        {
            throw new InvalidOperationException("JWT SecretKey was not found.");
        }

        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.EventsType = typeof(JwtSessionValidator);
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtConfiguration.Issuer,
                    ValidAudience = jwtConfiguration.Audience,

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtConfiguration.SecretKey)),

                    ClockSkew = TimeSpan.Zero
                };
            });
        builder.Services.AddScoped<JwtSessionValidator>();
        builder.Services.AddAuthorizationBuilder()
            .SetFallbackPolicy(new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build());
        // Services
        builder.Services.AddTransient<IEmailService, OutlookGraphEmailService>();

        builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
        builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

        builder.Services.AddScoped<ILoginUserRepository, LoginUserRepository>();
        builder.Services.AddScoped<IAuditRepository, AuditRepository>();

        builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
        builder.Services.AddScoped<IAuditService, AuditService>();
        builder.Services.AddScoped<IPermissionService, PermissionService>();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
        builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
        return builder;
    }
}
