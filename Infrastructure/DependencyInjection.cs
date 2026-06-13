namespace Infrastructure;

using Domain.Models.ConfigurationModels;
using Infrastructure.Data;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        builder.Services.Configure<BrevoConfiguration>(
            builder.Configuration.GetSection(BrevoConfiguration.SectionName)
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

        // Services
        builder.Services.AddTransient<IEmailService, BrevoEmailService>();


        return builder;
    }
}
