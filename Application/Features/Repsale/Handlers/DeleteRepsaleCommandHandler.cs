using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using LanguageExt.Common;

namespace Application.Features.Repsales;

public class DeleteRepsaleCommandHandler : IRequestHandler<DeleteRepsaleCommand, Result<Unit>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<DeleteRepsaleCommandHandler> _logger;

    public DeleteRepsaleCommandHandler(AppDbContext context, ILogger<DeleteRepsaleCommandHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Unit>> Handle(DeleteRepsaleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(DeleteRepsaleCommandHandler));

        try
        {
            var item = await _context.Repsales.FindAsync(new object[] { request.Id }, cancellationToken);
            if (item == null)
            {
                _logger.LogWarning("Repsale not found. Handler: {HandlerName}.", nameof(DeleteRepsaleCommandHandler));
                throw new KeyNotFoundException("Repsale not found");
            }
            _logger.LogInformation("Deleting {EntityName} with id {Id}. Handler: {HandlerName}.", "Repsale", request.Id, nameof(DeleteRepsaleCommandHandler));
            _context.Repsales.Remove(item);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(DeleteRepsaleCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling DeleteRepsaleCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(DeleteRepsaleCommandHandler));
                return new Result<Unit>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(DeleteRepsaleCommandHandler));
            return new Result<Unit>(Unit.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(DeleteRepsaleCommandHandler));
            return new Result<Unit>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(DeleteRepsaleCommandHandler));
        }
    }
}