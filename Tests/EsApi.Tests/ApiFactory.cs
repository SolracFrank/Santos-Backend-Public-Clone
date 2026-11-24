using System.Net.Http.Headers;
using System.Security.Cryptography;
using Domain.Common.Security;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EsApi.Tests;

internal sealed class ApiFactory : WebApplicationFactory<Program>
{
    private readonly string _database = Guid.NewGuid().ToString();

    public HttpClient CreateHttpsClient() => CreateClient(new()
    {
        BaseAddress = new Uri("https://localhost"),
        AllowAutoRedirect = false
    });

    public async Task<int> AuthenticateAsync(HttpClient client, string role, bool canCreateUsers = false)
    {
        var user = await SeedUserAsync(role, configure: user => user.CanCreateUsers = canCreateUsers);
        SetToken(client, user);
        return user.Idloginuser;
    }

    public async Task<LoginUser> SeedUserAsync(
        string role, string? password = null, Action<LoginUser>? configure = null)
    {
        using var scope = Services.CreateScope();
        var user = new LoginUser
        {
            Username = Guid.NewGuid().ToString(),
            PasswordHash = password is null
                ? string.Empty
                : scope.ServiceProvider.GetRequiredService<IPasswordHasher>().HashPassword(password),
            UserRole = role,
            IsActive = true,
            SecurityStamp = Guid.NewGuid()
        };
        configure?.Invoke(user);
        var database = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        database.LoginUsers.Add(user);
        await database.SaveChangesAsync();
        return user;
    }

    public void SetToken(HttpClient client, LoginUser user)
    {
        using var scope = Services.CreateScope();
        var token = scope.ServiceProvider.GetRequiredService<IJwtTokenService>().GenerateToken(user);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.UseSetting("Jwt:SecretKey", Convert.ToHexString(RandomNumberGenerator.GetBytes(32)));
        builder.UseSetting("Jwt:Issuer", "test-issuer");
        builder.UseSetting("Jwt:Audience", "test-audience");
        builder.UseSetting("API_BOOTSTRAP_ADMIN_PASSWORD", string.Empty);
        builder.UseSetting("Sentry:Dsn", string.Empty);
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IDbContextOptionsConfiguration<AppDbContext>>();
            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase(_database)
                .ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning)));
        });
    }
}
