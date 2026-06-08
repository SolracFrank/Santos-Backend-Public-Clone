using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using LanguageExt.Common;

namespace Application.Features.PaymentApplies;

public class DeletePaymentApplyCommandHandler : IRequestHandler<DeletePaymentApplyCommand, Result<Unit>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<DeletePaymentApplyCommandHandler> _logger;

    public DeletePaymentApplyCommandHandler(AppDbContext context, ILogger<DeletePaymentApplyCommandHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Unit>> Handle(DeletePaymentApplyCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(DeletePaymentApplyCommandHandler));

        try
        {
            var item = await _context.PaymentApplies.FindAsync(new object[] { request.Id }, cancellationToken);
            if (item == null)
            {
                _logger.LogWarning("PaymentApply not found. Handler: {HandlerName}.", nameof(DeletePaymentApplyCommandHandler));
                throw new KeyNotFoundException("PaymentApply not found");
            }
            _logger.LogInformation("Deleting {EntityName} with id {Id}. Handler: {HandlerName}.", "PaymentApply", request.Id, nameof(DeletePaymentApplyCommandHandler));
            _context.PaymentApplies.Remove(item);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(DeletePaymentApplyCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling DeletePaymentApplyCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(DeletePaymentApplyCommandHandler));
                return new Result<Unit>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(DeletePaymentApplyCommandHandler));
            return new Result<Unit>(Unit.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(DeletePaymentApplyCommandHandler));
            return new Result<Unit>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(DeletePaymentApplyCommandHandler));
        }
    }
}