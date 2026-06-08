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

public class GetPaymentAppliesQueryHandler : IRequestHandler<GetPaymentAppliesQuery, Result<List<PaymentApply>>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetPaymentAppliesQueryHandler> _logger;

    public GetPaymentAppliesQueryHandler(AppDbContext context, ILogger<GetPaymentAppliesQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<List<PaymentApply>>> Handle(GetPaymentAppliesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetPaymentAppliesQueryHandler));

        try
        {
            _logger.LogInformation("Getting list for {EntityName}. Handler: {HandlerName}.", "PaymentApplie", nameof(GetPaymentAppliesQueryHandler));
            return new Result<List<PaymentApply>>(await _context.PaymentApplies.ToListAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetPaymentAppliesQueryHandler));
            return new Result<List<PaymentApply>>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetPaymentAppliesQueryHandler));
        }
    }
}