using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using LanguageExt.Common;

namespace Application.Features.Addresses;

public class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand, Result<Unit>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<DeleteAddressCommandHandler> _logger;

    public DeleteAddressCommandHandler(AppDbContext context, ILogger<DeleteAddressCommandHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Unit>> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(DeleteAddressCommandHandler));

        try
        {
            var item = await _context.Addresses.FindAsync(new object[] { request.Id }, cancellationToken);
            if (item == null)
            {
                _logger.LogWarning("Address not found. Handler: {HandlerName}.", nameof(DeleteAddressCommandHandler));
                throw new KeyNotFoundException("Address not found");
            }
            _logger.LogInformation("Deleting {EntityName} with id {Id}. Handler: {HandlerName}.", "Addres", request.Id, nameof(DeleteAddressCommandHandler));
            _context.Addresses.Remove(item);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(DeleteAddressCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling DeleteAddressCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(DeleteAddressCommandHandler));
                return new Result<Unit>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(DeleteAddressCommandHandler));
            return new Result<Unit>(Unit.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(DeleteAddressCommandHandler));
            return new Result<Unit>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(DeleteAddressCommandHandler));
        }
    }
}