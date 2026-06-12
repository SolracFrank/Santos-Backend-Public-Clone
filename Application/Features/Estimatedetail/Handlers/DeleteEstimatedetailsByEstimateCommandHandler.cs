using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Estimatedetails;

public class DeleteEstimatedetailsByEstimateCommandHandler : IRequestHandler<DeleteEstimatedetailsByEstimateCommand, Result<Unit>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<DeleteEstimatedetailsByEstimateCommandHandler> _logger;

    public DeleteEstimatedetailsByEstimateCommandHandler(
        AppDbContext context,
        ILogger<DeleteEstimatedetailsByEstimateCommandHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Unit>> Handle(DeleteEstimatedetailsByEstimateCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(DeleteEstimatedetailsByEstimateCommandHandler));

        try
        {
            var items = await _context.Estimatedetails
                .Where(x => x.Idestimate == request.IdEstimate)
                .ToListAsync(cancellationToken);

            if (!items.Any())
            {
                _logger.LogInformation(
                    "No estimate details found to delete for EstimateId {EstimateId}.",
                    request.IdEstimate);

                return new Result<Unit>(Unit.Value);
            }

            _context.Estimatedetails.RemoveRange(items);

            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(DeleteEstimatedetailsByEstimateCommandHandler));

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(DeleteEstimatedetailsByEstimateCommandHandler));

            return new Result<Unit>(Unit.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(DeleteEstimatedetailsByEstimateCommandHandler));
            return new Result<Unit>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(DeleteEstimatedetailsByEstimateCommandHandler));
        }
    }
}