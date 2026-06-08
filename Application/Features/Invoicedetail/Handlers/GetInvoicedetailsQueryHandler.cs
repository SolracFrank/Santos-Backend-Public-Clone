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

public class GetInvoicedetailsQueryHandler : IRequestHandler<GetInvoicedetailsQuery, Result<List<Invoicedetail>>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetInvoicedetailsQueryHandler> _logger;

    public GetInvoicedetailsQueryHandler(AppDbContext context, ILogger<GetInvoicedetailsQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<List<Invoicedetail>>> Handle(GetInvoicedetailsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetInvoicedetailsQueryHandler));

        try
        {
            _logger.LogInformation("Getting list for {EntityName}. Handler: {HandlerName}.", "Invoicedetail", nameof(GetInvoicedetailsQueryHandler));
            return new Result<List<Invoicedetail>>(await _context.Invoicedetails.ToListAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetInvoicedetailsQueryHandler));
            return new Result<List<Invoicedetail>>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetInvoicedetailsQueryHandler));
        }
    }
}