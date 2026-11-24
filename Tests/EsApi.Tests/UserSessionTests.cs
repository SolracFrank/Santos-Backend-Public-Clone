using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using Domain.Entities;
using Domain.Entities.Dtos;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EsApi.Tests;

public sealed class UserSessionTests
{
    [Fact]
    public async Task ResetPasswordRevokesTheOldTokenAndOnlyTheNewPasswordCanLogIn()
    {
        using var app = new ApiFactory();
        using var administratorClient = app.CreateHttpsClient();
        using var staffClient = app.CreateHttpsClient();
        using var loginClient = app.CreateHttpsClient();
        var oldPassword = RandomPassword();
        var newPassword = RandomPassword();
        var staff = await app.SeedUserAsync("Staff", oldPassword);
        var administrator = await app.SeedUserAsync(LoginUser.AdministratorRole,
            configure: user => user.CanEditUsers = true);
        app.SetToken(administratorClient, administrator);

        using var initialLogin = await LogInAsync(loginClient, staff, oldPassword);
        var oldToken = await ReadTokenAsync(initialLogin);
        staffClient.DefaultRequestHeaders.Authorization = oldToken;
        using var initialRead = await staffClient.GetAsync("/api/v1/payments");
        Assert.Equal(HttpStatusCode.OK, initialRead.StatusCode);

        using var reset = await administratorClient.PutAsJsonAsync(
            $"/api/v1/loginusers/{staff.Idloginuser}/reset-password", new { newPassword });
        Assert.Equal(HttpStatusCode.OK, reset.StatusCode);

        using var revokedRead = await staffClient.GetAsync("/api/v1/payments");
        using var refusedLogin = await LogInAsync(loginClient, staff, oldPassword);
        Assert.Equal(HttpStatusCode.Unauthorized, revokedRead.StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, refusedLogin.StatusCode);

        using var renewedLogin = await LogInAsync(loginClient, staff, newPassword);
        staffClient.DefaultRequestHeaders.Authorization = await ReadTokenAsync(renewedLogin);
        using var renewedRead = await staffClient.GetAsync("/api/v1/payments");
        Assert.Equal(HttpStatusCode.OK, renewedRead.StatusCode);
    }

    [Fact]
    public async Task EnablingAnAccountDoesNotRestorePreviousSessions()
    {
        using var app = new ApiFactory();
        using var administratorClient = app.CreateHttpsClient();
        using var staffClient = app.CreateHttpsClient();
        var staff = await app.SeedUserAsync("Staff");
        var administrator = await app.SeedUserAsync(LoginUser.AdministratorRole,
            configure: user => user.CanDisableUsers = true);
        app.SetToken(administratorClient, administrator);
        app.SetToken(staffClient, staff);

        var statusRoute = $"/api/v1/loginusers/{staff.Idloginuser}/status";
        using var disabled = await administratorClient.PutAsJsonAsync(statusRoute, new { IsActive = false });
        using var deniedWhileDisabled = await staffClient.GetAsync("/api/v1/payments");
        using var enabled = await administratorClient.PutAsJsonAsync(statusRoute, new { IsActive = true });
        using var deniedAfterReactivation = await staffClient.GetAsync("/api/v1/payments");

        Assert.Equal(HttpStatusCode.OK, disabled.StatusCode);
        Assert.Equal(HttpStatusCode.OK, enabled.StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, deniedWhileDisabled.StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, deniedAfterReactivation.StatusCode);
    }

    [Fact]
    public async Task StaffCannotResetAnotherPrivilegedAccount()
    {
        using var app = new ApiFactory();
        using var client = app.CreateHttpsClient();
        var actor = await app.SeedUserAsync("Staff", configure: user => user.CanEditUsers = true);
        var target = await app.SeedUserAsync("Staff", RandomPassword(),
            user => user.CanManagePermissions = true);
        app.SetToken(client, actor);

        using var response = await client.PutAsJsonAsync(
            $"/api/v1/loginusers/{target.Idloginuser}/reset-password", new { NewPassword = RandomPassword() });
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);

        using var scope = app.Services.CreateScope();
        var database = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var unchanged = await database.LoginUsers.AsNoTracking()
            .SingleAsync(user => user.Idloginuser == target.Idloginuser);
        Assert.Equal(target.SecurityStamp, unchanged.SecurityStamp);
        Assert.Equal(target.PasswordHash, unchanged.PasswordHash);
    }

    [Theory]
    [InlineData(false, HttpStatusCode.OK)]
    [InlineData(true, HttpStatusCode.Forbidden)]
    public async Task UserCreatorCanCreateStaffButCannotDelegatePermissions(
        bool delegatesEditPermission, HttpStatusCode expectedStatus)
    {
        using var app = new ApiFactory();
        using var client = app.CreateHttpsClient();
        await app.AuthenticateAsync(client, "Staff", canCreateUsers: true);
        var username = Guid.NewGuid().ToString();
        using var response = await client.PostAsJsonAsync("/api/v1/loginusers", new
        {
            Username = username,
            Password = RandomPassword(),
            UserRole = "Staff",
            IsActive = true,
            CanEditUsers = delegatesEditPermission
        });
        Assert.Equal(expectedStatus, response.StatusCode);

        using var scope = app.Services.CreateScope();
        var database = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var created = await database.LoginUsers.AsNoTracking().SingleOrDefaultAsync(user => user.Username == username);
        if (delegatesEditPermission)
            Assert.Null(created);
        else
        {
            Assert.NotNull(created);
            Assert.False(created.CanEditUsers);
            Assert.False(created.CanSendEmails);
        }
    }

    private static string RandomPassword() => Convert.ToHexString(RandomNumberGenerator.GetBytes(16));

    private static Task<HttpResponseMessage> LogInAsync(HttpClient client, LoginUser user, string password) =>
        client.PostAsJsonAsync("/api/v1/auth/login", new { user.Username, Password = password });

    private static async Task<AuthenticationHeaderValue> ReadTokenAsync(HttpResponseMessage response)
    {
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var login = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        Assert.NotNull(login);
        return new AuthenticationHeaderValue("Bearer", login.Token);
    }
}
