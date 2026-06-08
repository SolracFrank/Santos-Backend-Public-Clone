using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using LanguageExt.Common;

namespace Application.Features.Employees;

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Result<Employee>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateEmployeeCommandHandler> _logger;

    public CreateEmployeeCommandHandler(AppDbContext context, ILogger<CreateEmployeeCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Employee>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(CreateEmployeeCommandHandler));

        try
        {
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
            _logger.LogInformation("Creating {EntityName}. Handler: {HandlerName}.", nameof(Employee), nameof(CreateEmployeeCommandHandler));
            _context.Set<Employee>().Add(entity);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(CreateEmployeeCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling CreateEmployeeCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(CreateEmployeeCommandHandler));
                return new Result<Employee>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(CreateEmployeeCommandHandler));
            return new Result<Employee>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(CreateEmployeeCommandHandler));
            return new Result<Employee>(ex);
        }
    }
}