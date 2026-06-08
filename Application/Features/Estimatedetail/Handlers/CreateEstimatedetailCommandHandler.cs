using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using LanguageExt.Common;

namespace Application.Features.Estimatedetails;

public class CreateEstimatedetailCommandHandler : IRequestHandler<CreateEstimatedetailCommand, Result<Estimatedetail>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateEstimatedetailCommandHandler> _logger;

    public CreateEstimatedetailCommandHandler(AppDbContext context, ILogger<CreateEstimatedetailCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Estimatedetail>> Handle(CreateEstimatedetailCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(CreateEstimatedetailCommandHandler));

        try
        {
            var entity = new Estimatedetail
            {
                Idestimatedetail = request.Idestimatedetail,
                Idestimate = request.Idestimate,
                Iditem = request.Iditem,
                Itemname = request.Itemname,
                Description = request.Description,
                Qty = request.Qty,
                Cost = request.Cost,
                Total = request.Total
            };
            _logger.LogInformation("Creating {EntityName}. Handler: {HandlerName}.", nameof(Estimatedetail), nameof(CreateEstimatedetailCommandHandler));
            _context.Set<Estimatedetail>().Add(entity);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(CreateEstimatedetailCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling CreateEstimatedetailCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(CreateEstimatedetailCommandHandler));
                return new Result<Estimatedetail>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(CreateEstimatedetailCommandHandler));
            return new Result<Estimatedetail>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(CreateEstimatedetailCommandHandler));
            return new Result<Estimatedetail>(ex);
        }
    }
}