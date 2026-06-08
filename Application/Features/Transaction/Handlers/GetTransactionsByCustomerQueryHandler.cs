using Domain.Entities.Dtos;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using LanguageExt.Common;

namespace Application.Features.Transaction;

public class GetTransactionsByCustomerQueryHandler : IRequestHandler<GetTransactionsByCustomerQuery, Result<List<CustomerTransaction>>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetTransactionsByCustomerQueryHandler> _logger;

    public GetTransactionsByCustomerQueryHandler(AppDbContext context, ILogger<GetTransactionsByCustomerQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<List<CustomerTransaction>>> Handle(GetTransactionsByCustomerQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetTransactionsByCustomerQueryHandler));

        try
        {
            return new Result<List<CustomerTransaction>>(await _context.Estimates
                .Where(e => e.Idusers == request.IdUser)
                .Select(e => new CustomerTransaction
                {
                    Idestimate = e.Idestimate,
                    Type = "Estimate",
                    Num = e.Estimatenumber.ToString(),
                    Date = e.Estimatedate,
                    Account = "Estimate",
                    Amount = e.Total
                })
                .OrderByDescending(x => x.Date)
                .ToListAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetTransactionsByCustomerQueryHandler));
            return new Result<List<CustomerTransaction>>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetTransactionsByCustomerQueryHandler));
        }
    }
}