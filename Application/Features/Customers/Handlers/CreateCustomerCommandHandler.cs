using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using LanguageExt.Common;

namespace Application.Features.Customers;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Result<Customer>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateCustomerCommandHandler> _logger;

    public CreateCustomerCommandHandler(AppDbContext context, ILogger<CreateCustomerCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Customer>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(CreateCustomerCommandHandler));

        try
        {
            var entity = new Customer
            {
                Idcustom = request.Idcustom,
                Cname = request.Cname,
                Idcustomfk = request.Idcustomfk
            };
            _logger.LogInformation("Creating {EntityName}. Handler: {HandlerName}.", nameof(Customer), nameof(CreateCustomerCommandHandler));
            _context.Set<Customer>().Add(entity);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(CreateCustomerCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling CreateCustomerCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(CreateCustomerCommandHandler));
                return new Result<Customer>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(CreateCustomerCommandHandler));
            return new Result<Customer>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(CreateCustomerCommandHandler));
            return new Result<Customer>(ex);
        }
    }
}