using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using LanguageExt.Common;

namespace Application.Features.Jobtypes;

public class DeleteJobtypeCommandHandler : IRequestHandler<DeleteJobtypeCommand, Result<Unit>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<DeleteJobtypeCommandHandler> _logger;

    public DeleteJobtypeCommandHandler(AppDbContext context, ILogger<DeleteJobtypeCommandHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Unit>> Handle(DeleteJobtypeCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(DeleteJobtypeCommandHandler));

        try
        {
            var item = await _context.Jobtypes.FindAsync(new object[] { request.Id }, cancellationToken);
            if (item == null)
            {
                _logger.LogWarning("Jobtype not found. Handler: {HandlerName}.", nameof(DeleteJobtypeCommandHandler));
                throw new KeyNotFoundException("Jobtype not found");
            }
            _logger.LogInformation("Deleting {EntityName} with id {Id}. Handler: {HandlerName}.", "Jobtype", request.Id, nameof(DeleteJobtypeCommandHandler));
            _context.Jobtypes.Remove(item);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(DeleteJobtypeCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling DeleteJobtypeCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(DeleteJobtypeCommandHandler));
                return new Result<Unit>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(DeleteJobtypeCommandHandler));
            return new Result<Unit>(Unit.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(DeleteJobtypeCommandHandler));
            return new Result<Unit>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(DeleteJobtypeCommandHandler));
        }
    }
}