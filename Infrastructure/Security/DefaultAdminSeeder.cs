using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Security
{
    public static class DefaultAdminSeeder
    {
        public static async Task SeedAsync(AppDbContext context, string bootstrapPassword)
        {
            var hasUsers = await context.LoginUsers.AnyAsync();

            if (hasUsers)
                return;

            var admin = new LoginUser
            {
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(bootstrapPassword),
                UserRole = LoginUser.AdministratorRole,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                LastLogin = null,
                SecurityStamp = Guid.NewGuid()
            };

            admin.GrantAdministratorPermissions();

            context.LoginUsers.Add(admin);

            await context.SaveChangesAsync();
        }
    }
}
