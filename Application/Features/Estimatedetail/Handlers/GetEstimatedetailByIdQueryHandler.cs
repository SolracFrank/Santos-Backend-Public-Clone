using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Estimatedetails;

public class GetEstimatedetailByIdQueryHandler : IRequestHandler<GetEstimatedetailByIdQuery, Result<Estimatedetail?>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetEstimatedetailByIdQueryHandler> _logger;

    public GetEstimatedetailByIdQueryHandler(AppDbContext context, ILogger<GetEstimatedetailByIdQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Estimatedetail?>> Handle(GetEstimatedetailByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetEstimatedetailByIdQueryHandler));

        try
        {
            _logger.LogInformation("Getting {EntityName} by id {Id}. Handler: {HandlerName}.", "EstimatedetailById", request.Id, nameof(GetEstimatedetailByIdQueryHandler));
            var entity = await _context.Estimatedetails.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity is null)
            {
                var notFoundException = new KeyNotFoundException("Estimatedetail not found");
                _logger.LogWarning(notFoundException, "{EntityName} with id {Id} was not found. Handler: {HandlerName}.", "Estimatedetail", request.Id, nameof(GetEstimatedetailByIdQueryHandler));
                return new Result<Estimatedetail?>(notFoundException);
            }

            return new Result<Estimatedetail?>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetEstimatedetailByIdQueryHandler));
            return new Result<Estimatedetail?>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetEstimatedetailByIdQueryHandler));
        }
    }
}