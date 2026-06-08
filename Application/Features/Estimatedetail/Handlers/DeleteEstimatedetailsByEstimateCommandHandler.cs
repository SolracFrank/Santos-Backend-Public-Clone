using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Estimatedetails;

public class DeleteEstimatedetailsByEstimateCommandHandler : IRequestHandler<DeleteEstimatedetailsByEstimateCommand, Result<Unit>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<DeleteEstimatedetailsByEstimateCommandHandler> _logger;

    public DeleteEstimatedetailsByEstimateCommandHandler(AppDbContext context, ILogger<DeleteEstimatedetailsByEstimateCommandHandler> logger)
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
        
            _context.Estimatedetails.RemoveRange(items);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(DeleteEstimatedetailsByEstimateCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling DeleteEstimatedetailsByEstimateCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(DeleteEstimatedetailsByEstimateCommandHandler));
                return new Result<Unit>(saveException);
            }
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