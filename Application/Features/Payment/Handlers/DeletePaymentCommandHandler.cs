using Domain.Common.Audit;
using Domain.Common.Security;
using Domain.Entities;
using Domain.Entities.Dtos;
using Infrastructure.Data;
using LanguageExt.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Payments;

public class DeletePaymentCommandHandler
    : IRequestHandler<DeletePaymentCommand, Result<bool>>
{
    private readonly AppDbContext _context;
    private readonly IAuditService _auditService;
    private readonly ICurrentUserService _currentUserService;
    public DeletePaymentCommandHandler(
        AppDbContext context,
        IAuditService auditService,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _auditService = auditService;
        _currentUserService = currentUserService;

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

            int paymentNumber = payment.Paymentnumber;
            decimal amount = payment.Amount;
            int idPayment = payment.Idpayment;

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
                    invoice.LastModifiedUtc = DateTime.UtcNow;
                }

                _context.PaymentApplies.Remove(apply);
            }

            _context.Payments.Remove(payment);

            await _context.SaveChangesAsync(cancellationToken);

            await _auditService.LogAsync(new AuditLogDto
            {
                IdLoginUser = _currentUserService.IdLoginUser,
                Username = _currentUserService.Username,
                Action = AuditActions.PaymentDeleted,
                EntityName = "Payments",
                EntityId = idPayment.ToString(),
                Description = $"Deleted payment #{paymentNumber} for {amount:C2}.",
                PerformedByRole = _currentUserService.UserRole,
                Succeeded = true,
                Severity = AuditSeverity.Warning
            });

            return true;
        }
        catch (Exception ex)
        {
            return new Result<bool>(ex);
        }
    }
}