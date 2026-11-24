using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LanguageExt.Common;
using System.Data;

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

            await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            var entity = await _context.PaymentApplies.FindAsync([request.Id], cancellationToken)
                ?? throw new NotFoundException("Payment allocation not found.");
            await PaymentAllocationChecks.VerifyAsync(
                _context, request.Idpayment, request.Idinvoice, request.AmountApplied, request.Id, cancellationToken);
            entity.Idpayment = request.Idpayment;
            entity.Idinvoice = request.Idinvoice;
            entity.AmountApplied = request.AmountApplied;
            _logger.LogInformation("Updating {EntityName} with id {Id}. Handler: {HandlerName}.", "PaymentApply", request.Id, nameof(UpdatePaymentApplyCommandHandler));
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(UpdatePaymentApplyCommandHandler));
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
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
