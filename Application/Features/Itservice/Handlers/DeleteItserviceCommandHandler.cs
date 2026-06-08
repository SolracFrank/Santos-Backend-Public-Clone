using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using LanguageExt.Common;

namespace Application.Features.Itservices;

public class DeleteItserviceCommandHandler : IRequestHandler<DeleteItserviceCommand, Result<Unit>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<DeleteItserviceCommandHandler> _logger;

    public DeleteItserviceCommandHandler(AppDbContext context, ILogger<DeleteItserviceCommandHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Unit>> Handle(DeleteItserviceCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(DeleteItserviceCommandHandler));

        try
        {
            var item = await _context.Itservices.FindAsync(new object[] { request.Id }, cancellationToken);
            if (item == null)
            {
                _logger.LogWarning("Itservice not found. Handler: {HandlerName}.", nameof(DeleteItserviceCommandHandler));
                throw new KeyNotFoundException("Itservice not found");
            }
            _logger.LogInformation("Deleting {EntityName} with id {Id}. Handler: {HandlerName}.", "Itservice", request.Id, nameof(DeleteItserviceCommandHandler));
            _context.Itservices.Remove(item);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(DeleteItserviceCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling DeleteItserviceCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(DeleteItserviceCommandHandler));
                return new Result<Unit>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(DeleteItserviceCommandHandler));
            return new Result<Unit>(Unit.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(DeleteItserviceCommandHandler));
            return new Result<Unit>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(DeleteItserviceCommandHandler));
        }
    }
}