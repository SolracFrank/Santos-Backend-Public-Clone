using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Employees;

public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, Result<List<Employee>>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetEmployeesQueryHandler> _logger;

    public GetEmployeesQueryHandler(AppDbContext context, ILogger<GetEmployeesQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<List<Employee>>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetEmployeesQueryHandler));

        try
        {
            _logger.LogInformation("Getting list for {EntityName}. Handler: {HandlerName}.", "Employee", nameof(GetEmployeesQueryHandler));
            return new Result<List<Employee>>(await _context.Employees.ToListAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetEmployeesQueryHandler));
            return new Result<List<Employee>>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetEmployeesQueryHandler));
        }
    }
}