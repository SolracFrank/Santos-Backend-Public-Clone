using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Estimates;

public class GetEstimateExistsQueryHandler : IRequestHandler<GetEstimateExistsQuery, Result<bool>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetEstimateExistsQueryHandler> _logger;

    public GetEstimateExistsQueryHandler(AppDbContext context, ILogger<GetEstimateExistsQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<bool>> Handle(GetEstimateExistsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetEstimateExistsQueryHandler));

        try
        {
            return new Result<bool>(await _context.Estimates
                .AnyAsync(x => x.Estimatenumber == request.EstimateNumber, cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetEstimateExistsQueryHandler));
            return new Result<bool>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetEstimateExistsQueryHandler));
        }
    }
}