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

        // Services
        builder.Services.AddTransient<IEmailService, BrevoEmailService>();


        return builder;
    }
}
