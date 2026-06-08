using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.PaymentApplies;

public class GetPaymentAppliesByInvoiceQueryHandler : IRequestHandler<GetPaymentAppliesByInvoiceQuery, Result<List<PaymentApply>>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetPaymentAppliesByInvoiceQueryHandler> _logger;

    public GetPaymentAppliesByInvoiceQueryHandler(AppDbContext context, ILogger<GetPaymentAppliesByInvoiceQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<List<PaymentApply>>> Handle(GetPaymentAppliesByInvoiceQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetPaymentAppliesByInvoiceQueryHandler));

        try
        {
            return new Result<List<PaymentApply>>(await _context.PaymentApplies
                .Where(x => x.Idinvoice == request.IdInvoice)
                .ToListAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetPaymentAppliesByInvoiceQueryHandler));
            return new Result<List<PaymentApply>>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetPaymentAppliesByInvoiceQueryHandler));
        }
    }
}