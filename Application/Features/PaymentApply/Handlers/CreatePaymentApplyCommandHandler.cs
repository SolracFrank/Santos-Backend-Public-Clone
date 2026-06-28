using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using LanguageExt.Common;
using Domain.Common.Audit;
using Domain.Common.Security;
using Domain.Entities.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.PaymentApplies;

public class CreatePaymentApplyCommandHandler : IRequestHandler<CreatePaymentApplyCommand, Result<PaymentApply>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreatePaymentApplyCommandHandler> _logger;
    private readonly IAuditService _auditService;
    private readonly ICurrentUserService _currentUserService;

    public CreatePaymentApplyCommandHandler(
        AppDbContext context,
        ILogger<CreatePaymentApplyCommandHandler> logger,
        IAuditService auditService,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _logger = logger;
        _auditService = auditService;
        _currentUserService = currentUserService;
    }

    public async Task<Result<PaymentApply>> Handle(CreatePaymentApplyCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(CreatePaymentApplyCommandHandler));

        try
        {
            var entity = new PaymentApply
            {
                Idpaymentapply = request.Idpaymentapply,
                Idpayment = request.Idpayment,
                Idinvoice = request.Idinvoice,
                AmountApplied = request.AmountApplied
            };
            _logger.LogInformation("Creating {EntityName}. Handler: {HandlerName}.", nameof(PaymentApply), nameof(CreatePaymentApplyCommandHandler));
            _context.Set<PaymentApply>().Add(entity);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(CreatePaymentApplyCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling CreatePaymentApplyCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(CreatePaymentApplyCommandHandler));
                return new Result<PaymentApply>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(CreatePaymentApplyCommandHandler));

            var payment = await _context.Payments
    .FirstOrDefaultAsync(x => x.Idpayment == entity.Idpayment, cancellationToken);

            var invoice = await _context.Invoices
                .FirstOrDefaultAsync(x => x.Idinvoice == entity.Idinvoice, cancellationToken);

            await _auditService.LogAsync(new AuditLogDto
            {
                IdLoginUser = _currentUserService.IdLoginUser,
                Username = _currentUserService.Username,
                Action = AuditActions.PaymentApplied,
                EntityName = "PaymentApplies",
                EntityId = entity.Idpaymentapply.ToString(),
                Description =
                    invoice == null || payment == null
                        ? $"Applied payment amount {entity.AmountApplied:C2}."
                        : $"Applied {entity.AmountApplied:C2} from payment #{payment.Paymentnumber} to invoice #{invoice.Invoicenumber}.",
                PerformedByRole = _currentUserService.UserRole,
                Succeeded = true,
                Severity = AuditSeverity.Information
            });

            var cleanEntity = new PaymentApply
            {
                Idpaymentapply = entity.Idpaymentapply,
                Idpayment = entity.Idpayment,
                Idinvoice = entity.Idinvoice,
                AmountApplied = entity.AmountApplied
            };

            return new Result<PaymentApply>(cleanEntity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(CreatePaymentApplyCommandHandler));
            return new Result<PaymentApply>(ex);
        }
    }
}