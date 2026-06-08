using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using LanguageExt.Common;

namespace Application.Features.Payments;

public class DeletePaymentCommandHandler : IRequestHandler<DeletePaymentCommand, Result<Unit>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<DeletePaymentCommandHandler> _logger;

    public DeletePaymentCommandHandler(AppDbContext context, ILogger<DeletePaymentCommandHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Unit>> Handle(DeletePaymentCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(DeletePaymentCommandHandler));

        try
        {
            var item = await _context.Payments.FindAsync(new object[] { request.Id }, cancellationToken);
            if (item == null)
            {
                _logger.LogWarning("Payment not found. Handler: {HandlerName}.", nameof(DeletePaymentCommandHandler));
                throw new KeyNotFoundException("Payment not found");
            }
            _logger.LogInformation("Deleting {EntityName} with id {Id}. Handler: {HandlerName}.", "Payment", request.Id, nameof(DeletePaymentCommandHandler));
            _context.Payments.Remove(item);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(DeletePaymentCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling DeletePaymentCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(DeletePaymentCommandHandler));
                return new Result<Unit>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(DeletePaymentCommandHandler));
            return new Result<Unit>(Unit.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(DeletePaymentCommandHandler));
            return new Result<Unit>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(DeletePaymentCommandHandler));
        }
    }
}