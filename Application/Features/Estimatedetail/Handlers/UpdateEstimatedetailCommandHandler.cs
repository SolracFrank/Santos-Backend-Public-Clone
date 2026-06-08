using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LanguageExt.Common;

namespace Application.Features.Estimatedetails;

public class UpdateEstimatedetailCommandHandler : IRequestHandler<UpdateEstimatedetailCommand, Result<Estimatedetail>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<UpdateEstimatedetailCommandHandler> _logger;

    public UpdateEstimatedetailCommandHandler(AppDbContext context, ILogger<UpdateEstimatedetailCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Estimatedetail>> Handle(UpdateEstimatedetailCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(UpdateEstimatedetailCommandHandler));

        try
        {
            if (request.Id != request.Idestimatedetail)
                throw new ArgumentException("Id mismatch");

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
            _logger.LogInformation("Updating {EntityName} with id {Id}. Handler: {HandlerName}.", "Estimatedetail", request.Id, nameof(UpdateEstimatedetailCommandHandler));
            _context.Entry(entity).State = EntityState.Modified;
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(UpdateEstimatedetailCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling UpdateEstimatedetailCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(UpdateEstimatedetailCommandHandler));
                return new Result<Estimatedetail>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(UpdateEstimatedetailCommandHandler));
            return new Result<Estimatedetail>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(UpdateEstimatedetailCommandHandler));
            return new Result<Estimatedetail>(ex);
        }
    }
}