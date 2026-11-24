using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using Application.Common.Behaviors;
using Application.Features.LoginUsers.Commands;
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
using Microsoft.Extensions.Logging;
using Xunit;

namespace EsApi.Tests;

public sealed class ApiSecurityTests
{
    [Fact]
    public async Task AttachmentsRequireAdminAndDoNotReturnStoragePath()
    {
        using var app = new ApiFactory();
        using var client = app.CreateClient(new() { BaseAddress = new Uri("https://localhost") });
        var anonymous = await client.GetAsync("/api/v1/attachments");
        Assert.Equal(HttpStatusCode.Unauthorized, anonymous.StatusCode);

        await AuthenticateAsync(app, client, "Staff");
        var employee = await client.GetAsync("/api/v1/attachments");
        Assert.Equal(HttpStatusCode.Forbidden, employee.StatusCode);

        await AuthenticateAsync(app, client, LoginUser.AdministratorRole);
        var created = await client.PostAsJsonAsync("/api/v1/attachments", new
        {
            EntityType = "Invoice",
            EntityId = 42,
            FileName = "test.pdf",
            FilePath = "/internal/test.pdf"
        });
        Assert.Equal(HttpStatusCode.OK, created.StatusCode);
        var response = await client.GetAsync("/api/v1/attachments");
        response.EnsureSuccessStatusCode();
        Assert.DoesNotContain("/internal/test.pdf", await response.Content.ReadAsStringAsync());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task RevokedSessionsAreRejected(bool disableUser)
    {
        using var app = new ApiFactory();
        using var client = app.CreateClient(new() { BaseAddress = new Uri("https://localhost") });
        var userId = await AuthenticateAsync(app, client, LoginUser.AdministratorRole);
        var before = await client.GetAsync("/api/v1/attachments");
        Assert.Equal(HttpStatusCode.OK, before.StatusCode);
        using var scope = app.Services.CreateScope();
        var database = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var user = await database.LoginUsers.FindAsync(userId);
        if (disableUser)
            user!.IsActive = false;
        else
            user!.SecurityStamp = Guid.NewGuid();
        await database.SaveChangesAsync();

        var after = await client.GetAsync("/api/v1/attachments");
        Assert.Equal(HttpStatusCode.Unauthorized, after.StatusCode);
    }

    [Fact]
    public async Task UserCreatorCannotGrantAdministratorRole()
    {
        using var app = new ApiFactory();
        using var client = app.CreateClient(new() { BaseAddress = new Uri("https://localhost") });
        await AuthenticateAsync(app, client, "Staff", canCreateUsers: true);
        var response = await client.PostAsJsonAsync("/api/v1/loginusers", new
        {
            Username = "test-admin",
            Password = Convert.ToHexString(RandomNumberGenerator.GetBytes(16)),
            UserRole = LoginUser.AdministratorRole,
            IsActive = true
        });
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task PasswordResetPayloadDoesNotReachRequestLogs()
    {
        var value = Convert.ToHexString(RandomNumberGenerator.GetBytes(24));
        var logger = new RecordingLogger<LoggingBehavior<ResetPasswordCommand, int>>();
        var behavior = new LoggingBehavior<ResetPasswordCommand, int>(logger);
        var result = await behavior.Handle(new ResetPasswordCommand { Id = 1, NewPassword = value },
            _ => Task.FromResult(1), CancellationToken.None);

        Assert.Equal(1, result);
        Assert.NotEmpty(logger.Messages);
        Assert.All(logger.Messages, message => Assert.DoesNotContain(value, message));
    }

    [Fact]
    public async Task InvoiceBalanceAndPaymentAmountsAreValidated()
    {
        using var app = new ApiFactory();
        using var client = app.CreateClient(new() { BaseAddress = new Uri("https://localhost") });
        await AuthenticateAsync(app, client, "Staff");
        var unauthorizedWrite = await client.PostAsJsonAsync("/api/v1/payments", new { Amount = 50 });
        Assert.Equal(HttpStatusCode.Forbidden, unauthorizedWrite.StatusCode);

        await AuthenticateAsync(app, client, LoginUser.AdministratorRole);
        var invoice = await client.PostAsJsonAsync("/api/v1/invoices", new { Subtotal = 100, Total = 100, Openbalance = 101 });
        var payment = await client.PostAsJsonAsync("/api/v1/payments", new { Amount = -1 });
        Assert.Equal(HttpStatusCode.BadRequest, invoice.StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, payment.StatusCode);
    }

    [Fact]
    public async Task PaymentAllocationRejectsDifferentCustomersAndExcessAmounts()
    {
        using var app = new ApiFactory();
        using var client = app.CreateClient(new() { BaseAddress = new Uri("https://localhost") });
        await AuthenticateAsync(app, client, LoginUser.AdministratorRole);
        using var scope = app.Services.CreateScope();
        var database = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var payment = new Payment { Amount = 100, Idusers = 11 };
        var customerInvoice = new Invoice { Total = 150, Idusers = 11, Activeinvoice = true };
        var otherInvoice = new Invoice { Total = 150, Idusers = 12, Activeinvoice = true };
        database.AddRange(payment, customerInvoice, otherInvoice);
        await database.SaveChangesAsync();

        var mismatch = await client.PostAsJsonAsync("/api/v1/paymentapplies",
            new { payment.Idpayment, otherInvoice.Idinvoice, AmountApplied = 10 });
        var excess = await client.PostAsJsonAsync("/api/v1/paymentapplies",
            new { payment.Idpayment, customerInvoice.Idinvoice, AmountApplied = 101 });
        Assert.Equal(HttpStatusCode.Conflict, mismatch.StatusCode);
        Assert.Equal(HttpStatusCode.Conflict, excess.StatusCode);

        var accepted = await client.PostAsJsonAsync("/api/v1/paymentapplies",
            new { payment.Idpayment, customerInvoice.Idinvoice, AmountApplied = 80 });
        Assert.Equal(HttpStatusCode.OK, accepted.StatusCode);
        var overcommitted = await client.PostAsJsonAsync("/api/v1/paymentapplies",
            new { payment.Idpayment, customerInvoice.Idinvoice, AmountApplied = 21 });
        Assert.Equal(HttpStatusCode.Conflict, overcommitted.StatusCode);
        Assert.Equal(1, await database.PaymentApplies.CountAsync());
    }

    private static async Task<int> AuthenticateAsync(
        ApiFactory app, HttpClient client, string role, bool canCreateUsers = false)
    {
        using var scope = app.Services.CreateScope();
        var database = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var user = new LoginUser
        {
            Username = Guid.NewGuid().ToString(),
            PasswordHash = string.Empty,
            UserRole = role,
            IsActive = true,
            CanCreateUsers = canCreateUsers,
            SecurityStamp = Guid.NewGuid()
        };
        database.LoginUsers.Add(user);
        await database.SaveChangesAsync();
        var token = scope.ServiceProvider.GetRequiredService<IJwtTokenService>().GenerateToken(user);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
        return user.Idloginuser;
    }

    private sealed class ApiFactory : WebApplicationFactory<Program>
    {
        private readonly string _database = Guid.NewGuid().ToString();

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

    private sealed class RecordingLogger<T> : ILogger<T>
    {
        public List<string> Messages { get; } = [];

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            Messages.Add(state.ToString() ?? string.Empty);
            return null;
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
            Func<TState, Exception?, string> formatter) => Messages.Add(formatter(state, exception));
    }
}
