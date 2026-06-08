using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using LanguageExt.Common;

namespace Application.Features.Paymds;

public class DeletePaymdCommandHandler : IRequestHandler<DeletePaymdCommand, Result<Unit>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<DeletePaymdCommandHandler> _logger;

    public DeletePaymdCommandHandler(AppDbContext context, ILogger<DeletePaymdCommandHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Unit>> Handle(DeletePaymdCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(DeletePaymdCommandHandler));

        try
        {
            var item = await _context.Paymds.FindAsync(new object[] { request.Id }, cancellationToken);
            if (item == null)
            {
                _logger.LogWarning("Paymd not found. Handler: {HandlerName}.", nameof(DeletePaymdCommandHandler));
                throw new KeyNotFoundException("Paymd not found");
            }
            _logger.LogInformation("Deleting {EntityName} with id {Id}. Handler: {HandlerName}.", "Paymd", request.Id, nameof(DeletePaymdCommandHandler));
            _context.Paymds.Remove(item);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(DeletePaymdCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling DeletePaymdCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(DeletePaymdCommandHandler));
                return new Result<Unit>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(DeletePaymdCommandHandler));
            return new Result<Unit>(Unit.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(DeletePaymdCommandHandler));
            return new Result<Unit>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(DeletePaymdCommandHandler));
        }
    }
}