using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using LanguageExt.Common;

namespace Application.Features.Othernames;

public class DeleteOthernameCommandHandler : IRequestHandler<DeleteOthernameCommand, Result<Unit>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<DeleteOthernameCommandHandler> _logger;

    public DeleteOthernameCommandHandler(AppDbContext context, ILogger<DeleteOthernameCommandHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Unit>> Handle(DeleteOthernameCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(DeleteOthernameCommandHandler));

        try
        {
            var item = await _context.Othernames.FindAsync(new object[] { request.Id }, cancellationToken);
            if (item == null)
            {
                _logger.LogWarning("Othername not found. Handler: {HandlerName}.", nameof(DeleteOthernameCommandHandler));
                throw new KeyNotFoundException("Othername not found");
            }
            _logger.LogInformation("Deleting {EntityName} with id {Id}. Handler: {HandlerName}.", "Othername", request.Id, nameof(DeleteOthernameCommandHandler));
            _context.Othernames.Remove(item);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(DeleteOthernameCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling DeleteOthernameCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(DeleteOthernameCommandHandler));
                return new Result<Unit>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(DeleteOthernameCommandHandler));
            return new Result<Unit>(Unit.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(DeleteOthernameCommandHandler));
            return new Result<Unit>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(DeleteOthernameCommandHandler));
        }
    }
}