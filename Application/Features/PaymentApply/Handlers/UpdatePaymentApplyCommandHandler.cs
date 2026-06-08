using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LanguageExt.Common;

namespace Application.Features.PaymentApplies;

public class UpdatePaymentApplyCommandHandler : IRequestHandler<UpdatePaymentApplyCommand, Result<PaymentApply>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<UpdatePaymentApplyCommandHandler> _logger;

    public UpdatePaymentApplyCommandHandler(AppDbContext context, ILogger<UpdatePaymentApplyCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<PaymentApply>> Handle(UpdatePaymentApplyCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(UpdatePaymentApplyCommandHandler));

        try
        {
            if (request.Id != request.Idpaymentapply)
                throw new ArgumentException("Id mismatch");

            var entity = new PaymentApply
            {
                Idpaymentapply = request.Idpaymentapply,
                Idpayment = request.Idpayment,
                Idinvoice = request.Idinvoice,
                AmountApplied = request.AmountApplied
            };
            _logger.LogInformation("Updating {EntityName} with id {Id}. Handler: {HandlerName}.", "PaymentApply", request.Id, nameof(UpdatePaymentApplyCommandHandler));
            _context.Entry(entity).State = EntityState.Modified;
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(UpdatePaymentApplyCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling UpdatePaymentApplyCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(UpdatePaymentApplyCommandHandler));
                return new Result<PaymentApply>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(UpdatePaymentApplyCommandHandler));
            return new Result<PaymentApply>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(UpdatePaymentApplyCommandHandler));
            return new Result<PaymentApply>(ex);
        }
    }
}