using System.Net;
using System.Net.Http.Json;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EsApi.Tests;

public sealed class PaymentAllocationUpdateTests
{
    [Fact]
    public async Task UpdatingAnAllocationExcludesItsOldAmountAndKeepsOtherAllocations()
    {
        using var app = new ApiFactory();
        using var client = app.CreateHttpsClient();
        await app.AuthenticateAsync(client, LoginUser.AdministratorRole);

        using var scope = app.Services.CreateScope();
        var database = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var (payment, invoice) = await AddPaymentAndInvoiceAsync(database);
        var otherInvoice = CreateInvoice();
        database.Invoices.Add(otherInvoice);
        await database.SaveChangesAsync();
        var primary = await AddAllocationAsync(database, payment, invoice, 60);
        var other = await AddAllocationAsync(database, payment, otherInvoice, 20);

        using var allowed = await UpdateAllocationAsync(client, primary, 80);
        using var rejected = await UpdateAllocationAsync(client, primary, 81);
        Assert.Equal(HttpStatusCode.OK, allowed.StatusCode);
        Assert.Equal(HttpStatusCode.Conflict, rejected.StatusCode);

        var persisted = await database.PaymentApplies.AsNoTracking().ToDictionaryAsync(
            allocation => allocation.Idpaymentapply, allocation => allocation.AmountApplied);
        Assert.Equal(80m, persisted[primary.Idpaymentapply]);
        Assert.Equal(20m, persisted[other.Idpaymentapply]);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task AllocatedPaymentRejectsChangesToItsCustomerOrAvailableFunds(bool changeCustomer)
    {
        using var app = new ApiFactory();
        using var client = app.CreateHttpsClient();
        await app.AuthenticateAsync(client, LoginUser.AdministratorRole);

        using var scope = app.Services.CreateScope();
        var database = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var (payment, invoice) = await AddPaymentAndInvoiceAsync(database);
        await AddAllocationAsync(database, payment, invoice, 80);

        using var response = await client.PutAsJsonAsync($"/api/v1/payments/{payment.Idpayment}", new
        {
            Id = payment.Idpayment,
            payment.Idpayment,
            payment.Paymentdate,
            payment.Paymentnumber,
            Amount = changeCustomer ? payment.Amount : 79m,
            Idusers = changeCustomer ? 12 : payment.Idusers
        });

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        var unchanged = await database.Payments.AsNoTracking()
            .SingleAsync(saved => saved.Idpayment == payment.Idpayment);
        Assert.Equal(payment.Idusers, unchanged.Idusers);
        Assert.Equal(payment.Amount, unchanged.Amount);
        Assert.Equal(80m, await database.PaymentApplies.SumAsync(allocation => allocation.AmountApplied));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task AllocatedInvoiceRejectsChangesToItsCustomerOrTotal(bool changeCustomer)
    {
        using var app = new ApiFactory();
        using var client = app.CreateHttpsClient();
        await app.AuthenticateAsync(client, LoginUser.AdministratorRole);

        using var scope = app.Services.CreateScope();
        var database = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var (payment, invoice) = await AddPaymentAndInvoiceAsync(database);
        await AddAllocationAsync(database, payment, invoice, 80);

        using var response = await client.PutAsJsonAsync($"/api/v1/invoices/{invoice.Idinvoice}", new
        {
            Id = invoice.Idinvoice,
            invoice.Idinvoice,
            invoice.Invoicedate,
            invoice.Invoicenumber,
            invoice.Activeinvoice,
            invoice.LastModifiedUtc,
            invoice.Openbalance,
            Total = changeCustomer ? invoice.Total : 79m,
            Subtotal = changeCustomer ? invoice.Subtotal : 79m,
            Idusers = changeCustomer ? 12 : invoice.Idusers
        });

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        var unchanged = await database.Invoices.AsNoTracking()
            .SingleAsync(saved => saved.Idinvoice == invoice.Idinvoice);
        Assert.Equal(invoice.Idusers, unchanged.Idusers);
        Assert.Equal(invoice.Total, unchanged.Total);
    }

    private static Invoice CreateInvoice() => new()
    {
        Subtotal = 150,
        Total = 150,
        Openbalance = 20,
        Idusers = 11,
        Activeinvoice = true,
        Invoicedate = DateTime.UtcNow.Date,
        LastModifiedUtc = DateTime.UtcNow
    };

    private static async Task<(Payment Payment, Invoice Invoice)> AddPaymentAndInvoiceAsync(AppDbContext database)
    {
        var payment = new Payment
        {
            Paymentnumber = 1,
            Amount = 100,
            Idusers = 11,
            Paymentdate = DateTime.UtcNow.Date
        };
        var invoice = CreateInvoice();
        database.AddRange(payment, invoice);
        await database.SaveChangesAsync();
        return (payment, invoice);
    }

    private static async Task<PaymentApply> AddAllocationAsync(
        AppDbContext database, Payment payment, Invoice invoice, decimal amount)
    {
        var allocation = new PaymentApply
        {
            Idpayment = payment.Idpayment,
            Idinvoice = invoice.Idinvoice,
            AmountApplied = amount
        };
        database.PaymentApplies.Add(allocation);
        await database.SaveChangesAsync();
        return allocation;
    }

    private static Task<HttpResponseMessage> UpdateAllocationAsync(
        HttpClient client, PaymentApply allocation, decimal amount) =>
        client.PutAsJsonAsync($"/api/v1/paymentapplies/{allocation.Idpaymentapply}", new
        {
            Id = allocation.Idpaymentapply,
            allocation.Idpaymentapply,
            allocation.Idpayment,
            allocation.Idinvoice,
            AmountApplied = amount
        });
}
