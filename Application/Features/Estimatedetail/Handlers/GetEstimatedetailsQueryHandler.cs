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

public class GetEstimatedetailsQueryHandler : IRequestHandler<GetEstimatedetailsQuery, Result<List<Estimatedetail>>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetEstimatedetailsQueryHandler> _logger;

    public GetEstimatedetailsQueryHandler(AppDbContext context, ILogger<GetEstimatedetailsQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<List<Estimatedetail>>> Handle(GetEstimatedetailsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetEstimatedetailsQueryHandler));

        try
        {
            _logger.LogInformation("Getting list for {EntityName}. Handler: {HandlerName}.", "Estimatedetail", nameof(GetEstimatedetailsQueryHandler));
            return new Result<List<Estimatedetail>>(await _context.Estimatedetails.ToListAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetEstimatedetailsQueryHandler));
            return new Result<List<Estimatedetail>>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetEstimatedetailsQueryHandler));
        }
    }
}