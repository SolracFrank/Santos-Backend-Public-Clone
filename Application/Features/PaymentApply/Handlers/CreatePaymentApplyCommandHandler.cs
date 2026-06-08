using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using LanguageExt.Common;

namespace Application.Features.PaymentApplies;

public class CreatePaymentApplyCommandHandler : IRequestHandler<CreatePaymentApplyCommand, Result<PaymentApply>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreatePaymentApplyCommandHandler> _logger;

    public CreatePaymentApplyCommandHandler(AppDbContext context, ILogger<CreatePaymentApplyCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
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
            return new Result<PaymentApply>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(CreatePaymentApplyCommandHandler));
            return new Result<PaymentApply>(ex);
        }
    }
}