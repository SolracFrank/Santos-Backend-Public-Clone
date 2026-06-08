using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Estimates;

public class GetEstimatesQueryHandler : IRequestHandler<GetEstimatesQuery, Result<List<Estimate>>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetEstimatesQueryHandler> _logger;

    public GetEstimatesQueryHandler(AppDbContext context, ILogger<GetEstimatesQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<List<Estimate>>> Handle(GetEstimatesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetEstimatesQueryHandler));

        try
        {
            _logger.LogInformation("Getting list for {EntityName}. Handler: {HandlerName}.", "Estimate", nameof(GetEstimatesQueryHandler));
            return new Result<List<Estimate>>(await _context.Estimates.ToListAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetEstimatesQueryHandler));
            return new Result<List<Estimate>>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetEstimatesQueryHandler));
        }
    }
}