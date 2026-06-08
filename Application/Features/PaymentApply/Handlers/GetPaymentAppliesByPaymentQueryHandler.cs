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

public class GetPaymentAppliesByPaymentQueryHandler : IRequestHandler<GetPaymentAppliesByPaymentQuery, Result<List<PaymentApply>>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetPaymentAppliesByPaymentQueryHandler> _logger;

    public GetPaymentAppliesByPaymentQueryHandler(AppDbContext context, ILogger<GetPaymentAppliesByPaymentQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<List<PaymentApply>>> Handle(GetPaymentAppliesByPaymentQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetPaymentAppliesByPaymentQueryHandler));

        try
        {
            return new Result<List<PaymentApply>>(await _context.PaymentApplies
                .Where(x => x.Idpayment == request.IdPayment)
                .ToListAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetPaymentAppliesByPaymentQueryHandler));
            return new Result<List<PaymentApply>>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetPaymentAppliesByPaymentQueryHandler));
        }
    }
}