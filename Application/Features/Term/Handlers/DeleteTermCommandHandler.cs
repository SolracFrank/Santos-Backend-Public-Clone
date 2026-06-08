using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using LanguageExt.Common;

namespace Application.Features.Terms;

public class DeleteTermCommandHandler : IRequestHandler<DeleteTermCommand, Result<Unit>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<DeleteTermCommandHandler> _logger;

    public DeleteTermCommandHandler(AppDbContext context, ILogger<DeleteTermCommandHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Unit>> Handle(DeleteTermCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(DeleteTermCommandHandler));

        try
        {
            var item = await _context.Terms.FindAsync(new object[] { request.Id }, cancellationToken);
            if (item == null)
            {
                _logger.LogWarning("Term not found. Handler: {HandlerName}.", nameof(DeleteTermCommandHandler));
                throw new KeyNotFoundException("Term not found");
            }
            _logger.LogInformation("Deleting {EntityName} with id {Id}. Handler: {HandlerName}.", "Term", request.Id, nameof(DeleteTermCommandHandler));
            _context.Terms.Remove(item);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(DeleteTermCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling DeleteTermCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(DeleteTermCommandHandler));
                return new Result<Unit>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(DeleteTermCommandHandler));
            return new Result<Unit>(Unit.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(DeleteTermCommandHandler));
            return new Result<Unit>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(DeleteTermCommandHandler));
        }
    }
}