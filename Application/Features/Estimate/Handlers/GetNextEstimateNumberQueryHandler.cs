using Application.Features.Estimate.Queries;
using Infrastructure.Data;
using LanguageExt.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Features.Estimate.Handlers;

public class GetNextEstimateNumberQueryHandler
    : IRequestHandler<GetNextEstimateNumberQuery, Result<int>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetNextEstimateNumberQueryHandler> _logger;

    public GetNextEstimateNumberQueryHandler(
        AppDbContext context,
        ILogger<GetNextEstimateNumberQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<int>> Handle(
        GetNextEstimateNumberQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            int lastNumber = await _context.Estimates
                .MaxAsync(x => (int?)x.Estimatenumber, cancellationToken) ?? 0;

            return new Result<int>(lastNumber + 1);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting next estimate number.");
            return new Result<int>(ex);
        }
    }
}