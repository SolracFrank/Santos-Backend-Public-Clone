using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using LanguageExt.Common;

namespace Application.Features.Accounttypes;

public class DeleteAccounttypeCommandHandler : IRequestHandler<DeleteAccounttypeCommand, Result<Unit>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<DeleteAccounttypeCommandHandler> _logger;

    public DeleteAccounttypeCommandHandler(AppDbContext context, ILogger<DeleteAccounttypeCommandHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Unit>> Handle(DeleteAccounttypeCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(DeleteAccounttypeCommandHandler));

        try
        {
            var item = await _context.Accounttypes.FindAsync(new object[] { request.Id }, cancellationToken);
            if (item == null)
            {
                _logger.LogWarning("Accounttype not found. Handler: {HandlerName}.", nameof(DeleteAccounttypeCommandHandler));
                throw new KeyNotFoundException("Accounttype not found");
            }
            _logger.LogInformation("Deleting {EntityName} with id {Id}. Handler: {HandlerName}.", "Accounttype", request.Id, nameof(DeleteAccounttypeCommandHandler));
            _context.Accounttypes.Remove(item);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(DeleteAccounttypeCommandHandler));

            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling DeleteAccounttypeCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(DeleteAccounttypeCommandHandler));
                return new Result<Unit>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(DeleteAccounttypeCommandHandler));
            return new Result<Unit>(Unit.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(DeleteAccounttypeCommandHandler));
            return new Result<Unit>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(DeleteAccounttypeCommandHandler));
        }
    }
}