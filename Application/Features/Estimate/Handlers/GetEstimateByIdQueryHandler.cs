using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Estimates;

public class GetEstimateByIdQueryHandler : IRequestHandler<GetEstimateByIdQuery, Result<Estimate?>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetEstimateByIdQueryHandler> _logger;

    public GetEstimateByIdQueryHandler(AppDbContext context, ILogger<GetEstimateByIdQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Estimate?>> Handle(GetEstimateByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetEstimateByIdQueryHandler));

        try
        {
            _logger.LogInformation("Getting {EntityName} by id {Id}. Handler: {HandlerName}.", "EstimateById", request.Id, nameof(GetEstimateByIdQueryHandler));
            var entity = await _context.Estimates.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity is null)
            {
                var notFoundException = new KeyNotFoundException("Estimate not found");
                _logger.LogWarning(notFoundException, "{EntityName} with id {Id} was not found. Handler: {HandlerName}.", "Estimate", request.Id, nameof(GetEstimateByIdQueryHandler));
                return new Result<Estimate?>(notFoundException);
            }

            return new Result<Estimate?>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetEstimateByIdQueryHandler));
            return new Result<Estimate?>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetEstimateByIdQueryHandler));
        }
    }
}