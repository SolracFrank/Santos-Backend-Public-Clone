using Application.Features.Invoice.Queries;
using Infrastructure.Data;
using LanguageExt.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Features.Invoice.Handlers;

public class GetInvoiceByEstimateIdQueryHandler
    : IRequestHandler<GetInvoiceByEstimateIdQuery, Result<Domain.Entities.Invoice>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetInvoiceByEstimateIdQueryHandler> _logger;

    public GetInvoiceByEstimateIdQueryHandler(
        AppDbContext context,
        ILogger<GetInvoiceByEstimateIdQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Domain.Entities.Invoice>> Handle(
        GetInvoiceByEstimateIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var invoice = await _context.Set<Domain.Entities.Invoice>()
                .FirstOrDefaultAsync(
                    x => x.Createdfromestimateid == request.IdEstimate,
                    cancellationToken);

            if (invoice == null)
            {
                return new Result<Domain.Entities.Invoice>(
                    new KeyNotFoundException("Invoice not found for this estimate."));
            }

            return new Result<Domain.Entities.Invoice>(invoice);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting invoice by estimate id.");
            return new Result<Domain.Entities.Invoice>(ex);
        }
    }
}