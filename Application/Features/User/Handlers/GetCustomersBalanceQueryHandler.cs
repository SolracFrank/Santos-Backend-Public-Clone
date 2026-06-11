using Domain.Entities.Dtos;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Users;

public class GetCustomersBalanceQueryHandler : IRequestHandler<GetCustomersBalanceQuery, Result<List<CustomerBalanceDto>>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetCustomersBalanceQueryHandler> _logger;

    public GetCustomersBalanceQueryHandler(AppDbContext context, ILogger<GetCustomersBalanceQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<List<CustomerBalanceDto>>> Handle(GetCustomersBalanceQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetCustomersBalanceQueryHandler));

        try
        {
            var query = _context.Users
                .Select(u => new CustomerBalanceDto
                {
                    Idusers = u.Idusers,
                    Name = u.Ucustomer,
                    BalanceTotal =(_context.Invoices
                    .Where(i => i.Idusers == u.Idusers && i.Activeinvoice == true)
                    .Sum(i => (decimal?)i.Openbalance) ?? 0)
                });

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim();

                query = query.Where(x =>
                    (x.Name != null && x.Name.Contains(search)) ||

                    _context.Invoices.Any(i =>
                    i.Idusers == x.Idusers &&
                    i.Activeinvoice == true &&
                    i.Invoicenumber.ToString().Contains(search)) ||

                    _context.Estimates.Any(e =>
                    e.Idusers == x.Idusers &&
                    e.Activeestimate == true &&
                    e.Estimatenumber.ToString().Contains(search)) ||

                    _context.Payments.Any(p =>
                        p.Idusers == x.Idusers &&
                        p.Paymentnumber.ToString().Contains(search))
                );
            }

            query = request.OrderBy switch
            {
                "name" => query.OrderBy(x => x.Name),
                "name_desc" => query.OrderByDescending(x => x.Name),
                "balance" => query.OrderBy(x => x.BalanceTotal),
                "balance_desc" => query.OrderByDescending(x => x.BalanceTotal),
                _ => query.OrderBy(x => x.Name)
            };
        
            return new Result<List<CustomerBalanceDto>>(await query.ToListAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetCustomersBalanceQueryHandler));
            return new Result<List<CustomerBalanceDto>>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetCustomersBalanceQueryHandler));
        }
    }
}