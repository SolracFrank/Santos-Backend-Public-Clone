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
            var estimates = await _context.Estimates
            .Where(e => e.Idusers == request.IdUser && e.Activeestimate == true)
            .Select(e => new CustomerTransaction
            {
            Idestimate = e.Idestimate,
            Type = "Estimate",
            Num = e.Estimatenumber.ToString(),
            Date = e.Estimatedate,
            Account = "Estimate",
            Amount = e.Total,
                Status = _context.Invoices.Any(i =>
                i.Createdfromestimateid == e.Idestimate &&
                i.Activeinvoice == true)
                ? "Converted"
                : "Open"
            })
    .ToListAsync(cancellationToken);

            var invoices = await _context.Invoices
            .Where(i => i.Idusers == request.IdUser && i.Activeinvoice == true)
            .Select(i => new CustomerTransaction
            {
            Idinvoice = i.Idinvoice,
            Type = "Invoice",
            Num = i.Invoicenumber.ToString(),
            Date = i.Invoicedate,
            Account = "Invoice",
            Amount = i.Total,
            Status =
             i.Openbalance <= 0 ? "Paid" :
             i.Openbalance < i.Total ? "Partially Paid" :
             "Open"
            })
     .ToListAsync(cancellationToken);  

            var payments = await _context.Payments
                .Where(p => p.Idusers == request.IdUser)
                .Select(p => new CustomerTransaction
                {
                    Idpayment = p.Idpayment,
                    Type = "Payment",
                    Num = p.Paymentnumber.ToString(),
                    Date = p.Paymentdate,
                    Account = "Payment",
                    Amount = -p.Amount,
                    Status = ""
                })
                 .ToListAsync(cancellationToken);

            var transactions = estimates
                .Concat(invoices)
                .Concat(payments)
                .OrderByDescending(x => x.Date)
                .ToList();

            return new Result<List<CustomerTransaction>>(transactions);
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