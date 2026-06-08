using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using LanguageExt.Common;

namespace Application.Features.Estates;

public class DeleteEstateCommandHandler : IRequestHandler<DeleteEstateCommand, Result<Unit>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<DeleteEstateCommandHandler> _logger;

    public DeleteEstateCommandHandler(AppDbContext context, ILogger<DeleteEstateCommandHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Unit>> Handle(DeleteEstateCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(DeleteEstateCommandHandler));

        try
        {
            var item = await _context.Estates.FindAsync(new object[] { request.Id }, cancellationToken);
            if (item == null)
            {
                _logger.LogWarning("Estate not found. Handler: {HandlerName}.", nameof(DeleteEstateCommandHandler));
                throw new KeyNotFoundException("Estate not found");
            }
            _logger.LogInformation("Deleting {EntityName} with id {Id}. Handler: {HandlerName}.", "Estate", request.Id, nameof(DeleteEstateCommandHandler));
            _context.Estates.Remove(item);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(DeleteEstateCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling DeleteEstateCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(DeleteEstateCommandHandler));
                return new Result<Unit>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(DeleteEstateCommandHandler));
            return new Result<Unit>(Unit.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(DeleteEstateCommandHandler));
            return new Result<Unit>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(DeleteEstateCommandHandler));
        }
    }
}