using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using LanguageExt.Common;

namespace Application.Features.Estimates;

public class CreateEstimateCommandHandler : IRequestHandler<CreateEstimateCommand, Result<Estimate>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateEstimateCommandHandler> _logger;

    public CreateEstimateCommandHandler(AppDbContext context, ILogger<CreateEstimateCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Estimate>> Handle(CreateEstimateCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(CreateEstimateCommandHandler));

        try
        {
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
            _logger.LogInformation("Creating {EntityName}. Handler: {HandlerName}.", nameof(Estimate), nameof(CreateEstimateCommandHandler));
            _context.Set<Estimate>().Add(entity);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(CreateEstimateCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling CreateEstimateCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(CreateEstimateCommandHandler));
                return new Result<Estimate>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(CreateEstimateCommandHandler));
            return new Result<Estimate>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(CreateEstimateCommandHandler));
            return new Result<Estimate>(ex);
        }
    }
}