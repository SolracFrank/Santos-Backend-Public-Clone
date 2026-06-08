using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using LanguageExt.Common;

namespace Application.Features.Payts;

public class DeletePaytCommandHandler : IRequestHandler<DeletePaytCommand, Result<Unit>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<DeletePaytCommandHandler> _logger;

    public DeletePaytCommandHandler(AppDbContext context, ILogger<DeletePaytCommandHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Unit>> Handle(DeletePaytCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(DeletePaytCommandHandler));

        try
        {
            var item = await _context.Payts.FindAsync(new object[] { request.Id }, cancellationToken);
            if (item == null)
            {
                _logger.LogWarning("Payt not found. Handler: {HandlerName}.", nameof(DeletePaytCommandHandler));
                throw new KeyNotFoundException("Payt not found");
            }
            _logger.LogInformation("Deleting {EntityName} with id {Id}. Handler: {HandlerName}.", "Payt", request.Id, nameof(DeletePaytCommandHandler));
            _context.Payts.Remove(item);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(DeletePaytCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling DeletePaytCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(DeletePaytCommandHandler));
                return new Result<Unit>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(DeletePaytCommandHandler));
            return new Result<Unit>(Unit.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(DeletePaytCommandHandler));
            return new Result<Unit>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(DeletePaytCommandHandler));
        }
    }
}