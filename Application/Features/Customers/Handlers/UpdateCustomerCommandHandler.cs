using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LanguageExt.Common;

namespace Application.Features.Customers;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Result<Customer>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<UpdateCustomerCommandHandler> _logger;

    public UpdateCustomerCommandHandler(AppDbContext context, ILogger<UpdateCustomerCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Customer>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(UpdateCustomerCommandHandler));

        try
        {
            if (request.Id != request.Idcustom)
                throw new ArgumentException("Id mismatch");

            var entity = new Customer
            {
                Idcustom = request.Idcustom,
                Cname = request.Cname,
                Idcustomfk = request.Idcustomfk
            };
            _logger.LogInformation("Updating {EntityName} with id {Id}. Handler: {HandlerName}.", "Customer", request.Id, nameof(UpdateCustomerCommandHandler));
            _context.Entry(entity).State = EntityState.Modified;
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(UpdateCustomerCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling UpdateCustomerCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(UpdateCustomerCommandHandler));
                return new Result<Customer>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(UpdateCustomerCommandHandler));
            return new Result<Customer>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(UpdateCustomerCommandHandler));
            return new Result<Customer>(ex);
        }
    }
}