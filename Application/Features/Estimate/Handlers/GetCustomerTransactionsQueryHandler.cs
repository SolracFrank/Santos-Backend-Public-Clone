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

namespace Application.Features.Estimates;

public class GetCustomerTransactionsQueryHandler : IRequestHandler<GetCustomerTransactionsQuery, Result<List<CustomerTransaction>>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetCustomerTransactionsQueryHandler> _logger;

    public GetCustomerTransactionsQueryHandler(AppDbContext context, ILogger<GetCustomerTransactionsQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<List<CustomerTransaction>>> Handle(GetCustomerTransactionsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetCustomerTransactionsQueryHandler));

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
                    Account = "Accounts Receivable",
                    Amount = e.Total
                })
                .OrderByDescending(x => x.Date)
                .ToListAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetCustomerTransactionsQueryHandler));
            return new Result<List<CustomerTransaction>>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetCustomerTransactionsQueryHandler));
        }
    }
}