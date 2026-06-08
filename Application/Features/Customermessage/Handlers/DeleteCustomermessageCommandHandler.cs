using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using LanguageExt.Common;

namespace Application.Features.Customermessages;

public class DeleteCustomermessageCommandHandler : IRequestHandler<DeleteCustomermessageCommand, Result<Unit>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<DeleteCustomermessageCommandHandler> _logger;

    public DeleteCustomermessageCommandHandler(AppDbContext context, ILogger<DeleteCustomermessageCommandHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Unit>> Handle(DeleteCustomermessageCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(DeleteCustomermessageCommandHandler));

        try
        {
            var item = await _context.Customermessages.FindAsync(new object[] { request.Id }, cancellationToken);
            if (item == null)
            {
                _logger.LogWarning("Customermessage not found. Handler: {HandlerName}.", nameof(DeleteCustomermessageCommandHandler));
                throw new KeyNotFoundException("Customermessage not found");
            }
            _logger.LogInformation("Deleting {EntityName} with id {Id}. Handler: {HandlerName}.", "Customermessage", request.Id, nameof(DeleteCustomermessageCommandHandler));
            _context.Customermessages.Remove(item);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(DeleteCustomermessageCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling DeleteCustomermessageCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(DeleteCustomermessageCommandHandler));
                return new Result<Unit>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(DeleteCustomermessageCommandHandler));
            return new Result<Unit>(Unit.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(DeleteCustomermessageCommandHandler));
            return new Result<Unit>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(DeleteCustomermessageCommandHandler));
        }
    }
}