using Infrastructure.Data;
using LanguageExt.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Payments;

public class DeletePaymentCommandHandler
    : IRequestHandler<DeletePaymentCommand, Result<bool>>
{
    private readonly AppDbContext _context;

    public DeletePaymentCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> Handle(
        DeletePaymentCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var payment = await _context.Payments
                .Include(p => p.PaymentApplies)
                .FirstOrDefaultAsync(
                    x => x.Idpayment == request.Id,
                    cancellationToken);

            if (payment == null)
                return new Result<bool>(
                    new KeyNotFoundException("Payment not found."));

            foreach (var apply in payment.PaymentApplies.ToList())
            {
                var invoice = await _context.Invoices
                    .FirstOrDefaultAsync(
                        x => x.Idinvoice == apply.Idinvoice,
                        cancellationToken);

                if (invoice != null)
                {
                    invoice.Openbalance += apply.AmountApplied;

                    if (invoice.Openbalance > invoice.Total)
                        invoice.Openbalance = invoice.Total;

                    invoice.Activeinvoice = invoice.Openbalance > 0;
                }

                _context.PaymentApplies.Remove(apply);
            }

            _context.Payments.Remove(payment);

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            return new Result<bool>(ex);
        }
    }
}