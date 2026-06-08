using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Estimatedetails;

public class GetEstimatedetailsByEstimateQueryHandler : IRequestHandler<GetEstimatedetailsByEstimateQuery, Result<List<Estimatedetail>>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetEstimatedetailsByEstimateQueryHandler> _logger;

    public GetEstimatedetailsByEstimateQueryHandler(AppDbContext context, ILogger<GetEstimatedetailsByEstimateQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<List<Estimatedetail>>> Handle(GetEstimatedetailsByEstimateQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetEstimatedetailsByEstimateQueryHandler));

        try
        {
            return new Result<List<Estimatedetail>>(await _context.Estimatedetails
                .Where(x => x.Idestimate == request.IdEstimate)
                .ToListAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetEstimatedetailsByEstimateQueryHandler));
            return new Result<List<Estimatedetail>>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetEstimatedetailsByEstimateQueryHandler));
        }
    }
}