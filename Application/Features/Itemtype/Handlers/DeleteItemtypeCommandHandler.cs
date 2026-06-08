using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using LanguageExt.Common;

namespace Application.Features.Itemtypes;

public class DeleteItemtypeCommandHandler : IRequestHandler<DeleteItemtypeCommand, Result<Unit>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<DeleteItemtypeCommandHandler> _logger;

    public DeleteItemtypeCommandHandler(AppDbContext context, ILogger<DeleteItemtypeCommandHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Unit>> Handle(DeleteItemtypeCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(DeleteItemtypeCommandHandler));

        try
        {
            var item = await _context.Itemtypes.FindAsync(new object[] { request.Id }, cancellationToken);
            if (item == null)
            {
                _logger.LogWarning("Itemtype not found. Handler: {HandlerName}.", nameof(DeleteItemtypeCommandHandler));
                throw new KeyNotFoundException("Itemtype not found");
            }
            _logger.LogInformation("Deleting {EntityName} with id {Id}. Handler: {HandlerName}.", "Itemtype", request.Id, nameof(DeleteItemtypeCommandHandler));
            _context.Itemtypes.Remove(item);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(DeleteItemtypeCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling DeleteItemtypeCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(DeleteItemtypeCommandHandler));
                return new Result<Unit>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(DeleteItemtypeCommandHandler));
            return new Result<Unit>(Unit.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(DeleteItemtypeCommandHandler));
            return new Result<Unit>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(DeleteItemtypeCommandHandler));
        }
    }
}