using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LanguageExt.Common;

namespace Application.Features.Employees;

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, Result<Employee>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<UpdateEmployeeCommandHandler> _logger;

    public UpdateEmployeeCommandHandler(AppDbContext context, ILogger<UpdateEmployeeCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Employee>> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(UpdateEmployeeCommandHandler));

        try
        {
            if (request.Id != request.Idemployee)
                throw new ArgumentException("Id mismatch");

            var entity = new Employee
            {
                Idemployee = request.Idemployee,
                Ename = request.Ename,
                Elastname = request.Elastname,
                Eaddress = request.Eaddress,
                Ecity = request.Ecity,
                Ezip = request.Ezip,
                Eprint = request.Eprint,
                Ephone = request.Ephone,
                Eemail = request.Eemail,
                Idstate = request.Idstate
            };
            _logger.LogInformation("Updating {EntityName} with id {Id}. Handler: {HandlerName}.", "Employee", request.Id, nameof(UpdateEmployeeCommandHandler));
            _context.Entry(entity).State = EntityState.Modified;
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(UpdateEmployeeCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling UpdateEmployeeCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(UpdateEmployeeCommandHandler));
                return new Result<Employee>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(UpdateEmployeeCommandHandler));
            return new Result<Employee>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(UpdateEmployeeCommandHandler));
            return new Result<Employee>(ex);
        }
    }
}