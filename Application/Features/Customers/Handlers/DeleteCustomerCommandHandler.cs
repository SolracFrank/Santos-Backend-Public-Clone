using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using LanguageExt.Common;

namespace Application.Features.Customers;

public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Result<Unit>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<DeleteCustomerCommandHandler> _logger;

    public DeleteCustomerCommandHandler(AppDbContext context, ILogger<DeleteCustomerCommandHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Unit>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(DeleteCustomerCommandHandler));

        try
        {
            var entity = await _context.Customers.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity == null)
                throw new KeyNotFoundException();
            _logger.LogInformation("Deleting {EntityName} with id {Id}. Handler: {HandlerName}.", "Customer", request.Id, nameof(DeleteCustomerCommandHandler));
            _context.Customers.Remove(entity);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(DeleteCustomerCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling {nameof(DeleteCustomerCommand)}.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(DeleteCustomerCommandHandler));
                return new Result<Unit>(saveException);
            }

            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(DeleteCustomerCommandHandler));
            return new Result<Unit>(Unit.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(DeleteCustomerCommandHandler));
            return new Result<Unit>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(DeleteCustomerCommandHandler));
        }
    }
}
