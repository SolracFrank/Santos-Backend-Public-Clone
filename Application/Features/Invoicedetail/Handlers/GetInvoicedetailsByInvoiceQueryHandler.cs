using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Invoicedetails;

public class GetInvoicedetailsByInvoiceQueryHandler : IRequestHandler<GetInvoicedetailsByInvoiceQuery, Result<List<Invoicedetail>>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetInvoicedetailsByInvoiceQueryHandler> _logger;

    public GetInvoicedetailsByInvoiceQueryHandler(AppDbContext context, ILogger<GetInvoicedetailsByInvoiceQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<List<Invoicedetail>>> Handle(GetInvoicedetailsByInvoiceQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetInvoicedetailsByInvoiceQueryHandler));

        try
        {
            return new Result<List<Invoicedetail>>(await _context.Invoicedetails
                .Where(x => x.Idinvoice == request.IdInvoice)
                .ToListAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetInvoicedetailsByInvoiceQueryHandler));
            return new Result<List<Invoicedetail>>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetInvoicedetailsByInvoiceQueryHandler));
        }
    }
}