using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LanguageExt.Common;

namespace Application.Features.Estimates;

public class UpdateEstimateCommandHandler : IRequestHandler<UpdateEstimateCommand, Result<Estimate>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<UpdateEstimateCommandHandler> _logger;

    public UpdateEstimateCommandHandler(AppDbContext context, ILogger<UpdateEstimateCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Estimate>> Handle(UpdateEstimateCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(UpdateEstimateCommandHandler));

        try
        {
            if (request.Id != request.Idestimate)
                throw new ArgumentException("Id mismatch");

            var entity = new Estimate
            {
                Idestimate = request.Idestimate,
                Estimatenumber = request.Estimatenumber,
                Estimatedate = request.Estimatedate,
                Expdate = request.Expdate,
                Datetobecompleted = request.Datetobecompleted,
                Dateordered = request.Dateordered,
                Memo = request.Memo,
                Subtotal = request.Subtotal,
                Markup = request.Markup,
                Total = request.Total,
                Activeestimate = request.Activeestimate,
                Openbalance = request.Openbalance,
                Idusers = request.Idusers,
                Idterms = request.Idterms,
                Idrepsales = request.Idrepsales,
                Idcustomermessage = request.Idcustomermessage,
                Customermessagetext = request.Customermessagetext,
                IsCopy = request.IsCopy,
                CopiedFromEstimateId = request.CopiedFromEstimateId
            };
            _logger.LogInformation("Updating {EntityName} with id {Id}. Handler: {HandlerName}.", "Estimate", request.Id, nameof(UpdateEstimateCommandHandler));
            _context.Entry(entity).State = EntityState.Modified;
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(UpdateEstimateCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling UpdateEstimateCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(UpdateEstimateCommandHandler));
                return new Result<Estimate>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(UpdateEstimateCommandHandler));
            return new Result<Estimate>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(UpdateEstimateCommandHandler));
            return new Result<Estimate>(ex);
        }
    }
}