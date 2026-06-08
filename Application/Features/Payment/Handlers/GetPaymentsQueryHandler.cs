using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Payments;

public class GetPaymentsQueryHandler : IRequestHandler<GetPaymentsQuery, Result<List<Payment>>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetPaymentsQueryHandler> _logger;

    public GetPaymentsQueryHandler(AppDbContext context, ILogger<GetPaymentsQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<List<Payment>>> Handle(GetPaymentsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetPaymentsQueryHandler));

        try
        {
            _logger.LogInformation("Getting list for {EntityName}. Handler: {HandlerName}.", "Payment", nameof(GetPaymentsQueryHandler));
            return new Result<List<Payment>>(await _context.Payments.ToListAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetPaymentsQueryHandler));
            return new Result<List<Payment>>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetPaymentsQueryHandler));
        }
    }
}