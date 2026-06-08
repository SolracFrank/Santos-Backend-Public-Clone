using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using LanguageExt.Common;

namespace Application.Features.Taxes;

public class DeleteTaxCommandHandler : IRequestHandler<DeleteTaxCommand, Result<Unit>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<DeleteTaxCommandHandler> _logger;

    public DeleteTaxCommandHandler(AppDbContext context, ILogger<DeleteTaxCommandHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Unit>> Handle(DeleteTaxCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(DeleteTaxCommandHandler));

        try
        {
            var item = await _context.Taxes.FindAsync(new object[] { request.Id }, cancellationToken);
            if (item == null)
            {
                _logger.LogWarning("Tax not found. Handler: {HandlerName}.", nameof(DeleteTaxCommandHandler));
                throw new KeyNotFoundException("Tax not found");
            }
            _logger.LogInformation("Deleting {EntityName} with id {Id}. Handler: {HandlerName}.", "Tax", request.Id, nameof(DeleteTaxCommandHandler));
            _context.Taxes.Remove(item);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(DeleteTaxCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling DeleteTaxCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(DeleteTaxCommandHandler));
                return new Result<Unit>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(DeleteTaxCommandHandler));
            return new Result<Unit>(Unit.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(DeleteTaxCommandHandler));
            return new Result<Unit>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(DeleteTaxCommandHandler));
        }
    }
}