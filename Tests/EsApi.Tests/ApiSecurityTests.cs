using System.Net;
using System.Net.Http.Json;
using System.Security.Cryptography;
using Application.Common.Behaviors;
using Application.Features.LoginUsers.Commands;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace EsApi.Tests;

public sealed class ApiSecurityTests
{
    [Fact]
    public async Task AttachmentsRequireAdminAndDoNotReturnStoragePath()
    {
        using var app = new ApiFactory();
        using var client = app.CreateHttpsClient();
        var anonymous = await client.GetAsync("/api/v1/attachments");
        Assert.Equal(HttpStatusCode.Unauthorized, anonymous.StatusCode);

        await app.AuthenticateAsync(client, "Staff");
        var employee = await client.GetAsync("/api/v1/attachments");
        Assert.Equal(HttpStatusCode.Forbidden, employee.StatusCode);

        await app.AuthenticateAsync(client, LoginUser.AdministratorRole);
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
        using var client = app.CreateHttpsClient();
        var userId = await app.AuthenticateAsync(client, LoginUser.AdministratorRole);
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
        using var client = app.CreateHttpsClient();
        await app.AuthenticateAsync(client, "Staff", canCreateUsers: true);
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
        using var client = app.CreateHttpsClient();
        await app.AuthenticateAsync(client, "Staff");
        var unauthorizedWrite = await client.PostAsJsonAsync("/api/v1/payments", new { Amount = 50 });
        Assert.Equal(HttpStatusCode.Forbidden, unauthorizedWrite.StatusCode);

        await app.AuthenticateAsync(client, LoginUser.AdministratorRole);
        var invoice = await client.PostAsJsonAsync("/api/v1/invoices", new { Subtotal = 100, Total = 100, Openbalance = 101 });
        var payment = await client.PostAsJsonAsync("/api/v1/payments", new { Amount = -1 });
        Assert.Equal(HttpStatusCode.BadRequest, invoice.StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, payment.StatusCode);
    }

    [Fact]
    public async Task PaymentAllocationRejectsDifferentCustomersAndExcessAmounts()
    {
        using var app = new ApiFactory();
        using var client = app.CreateHttpsClient();
        await app.AuthenticateAsync(client, LoginUser.AdministratorRole);
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
