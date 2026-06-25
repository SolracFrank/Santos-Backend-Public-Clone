using Infrastructure.Data;
using LanguageExt.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Features.Invoices;

public class GetNextInvoiceNumberQueryHandler
    : IRequestHandler<GetNextInvoiceNumberQuery, Result<int>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetNextInvoiceNumberQueryHandler> _logger;

    public GetNextInvoiceNumberQueryHandler(
        AppDbContext context,
        ILogger<GetNextInvoiceNumberQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<int>> Handle(
        GetNextInvoiceNumberQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            int lastNumber = await _context.Set<Domain.Entities.Invoice>()
                .MaxAsync(x => (int?)x.Invoicenumber, cancellationToken) ?? 0;

            return new Result<int>(lastNumber + 1);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting next invoice number.");
            return new Result<int>(ex);
        }
    }
}