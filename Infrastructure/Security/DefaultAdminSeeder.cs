using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Security
{
    public static class DefaultAdminSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            var bootstrapPassword = Environment.GetEnvironmentVariable("API_BOOTSTRAP_ADMIN_PASSWORD");
            if (string.IsNullOrWhiteSpace(bootstrapPassword))
                return;

            var hasUsers = await context.LoginUsers.AnyAsync();

            if (hasUsers)
                return;

            var admin = new LoginUser
            {
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(bootstrapPassword),
                UserRole = "Administrator",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                LastLogin = null,
                SecurityStamp = Guid.NewGuid(),

                CanCreateUsers = true,
                CanEditUsers = true,
                CanDisableUsers = true,
                CanViewAuditLog = true,
                CanManagePermissions = true
            };

            context.LoginUsers.Add(admin);

            await context.SaveChangesAsync();
        }
    }
}