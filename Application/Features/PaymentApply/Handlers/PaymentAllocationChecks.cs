using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using InvoiceEntity = Domain.Entities.Invoice;

namespace Application.Features.PaymentApplies;

internal static class PaymentAllocationChecks
{
    internal static async Task<(Payment Payment, InvoiceEntity Invoice)> VerifyAsync(
        AppDbContext context, int paymentId, int invoiceId, decimal amount, int? replacingId,
        CancellationToken cancellationToken)
    {
        var payment = await context.Payments.AsNoTracking()
            .SingleOrDefaultAsync(entry => entry.Idpayment == paymentId, cancellationToken)
            ?? throw new NotFoundException("Payment not found.");
        var invoice = await context.Invoices.AsNoTracking()
            .SingleOrDefaultAsync(entry => entry.Idinvoice == invoiceId, cancellationToken)
            ?? throw new NotFoundException("Invoice not found.");

        if (payment.Idusers is null || payment.Idusers != invoice.Idusers)
            throw new ConflictException("Payment and invoice must belong to the same customer.");

        var allocations = context.PaymentApplies.AsNoTracking();
        if (replacingId.HasValue)
            allocations = allocations.Where(entry => entry.Idpaymentapply != replacingId.Value);

        var paidFromPayment = await allocations.Where(entry => entry.Idpayment == paymentId)
            .SumAsync(entry => entry.AmountApplied, cancellationToken);
        var paidToInvoice = await allocations.Where(entry => entry.Idinvoice == invoiceId)
            .SumAsync(entry => entry.AmountApplied, cancellationToken);
        if (amount <= 0 || amount > payment.Amount - paidFromPayment || amount > invoice.Total - paidToInvoice)
            throw new ConflictException("The allocation exceeds the payment or invoice amount available.");

        return (payment, invoice);
    }
}
