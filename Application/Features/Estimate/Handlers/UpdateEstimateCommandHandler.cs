using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LanguageExt.Common;

namespace Application.Features.Estimates;

public class UpdateEstimateCommandHandler : IRequestHandler<UpdateEstimateCommand, Result<Domain.Entities.Estimate>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<UpdateEstimateCommandHandler> _logger;

    public UpdateEstimateCommandHandler(AppDbContext context, ILogger<UpdateEstimateCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Domain.Entities.Estimate>> Handle(UpdateEstimateCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(UpdateEstimateCommandHandler));

        try
        {
            if (request.Id <= 0)
                return new Result<Domain.Entities.Estimate>(new ArgumentException("Invalid estimate id."));

            var entity = await _context.Estimates
                .FirstOrDefaultAsync(x => x.Idestimate == request.Id, cancellationToken);

            if (entity == null)
                return new Result<Domain.Entities.Estimate>(new KeyNotFoundException($"Estimate {request.Id} was not found."));

            entity.Estimatenumber = request.Estimatenumber;
            entity.Estimatedate = request.Estimatedate;
            entity.Expdate = request.Expdate;
            entity.Datetobecompleted = request.Datetobecompleted;
            entity.Dateordered = request.Dateordered;
            entity.Memo = request.Memo;
            entity.Subtotal = request.Subtotal;
            entity.Markup = request.Markup;
            entity.Total = request.Total;
            entity.Activeestimate = request.Activeestimate;
            entity.Openbalance = request.Openbalance;
            entity.Idusers = request.Idusers;
            entity.Idterms = request.Idterms;
            entity.Idrepsales = request.Idrepsales;
            entity.Idcustomermessage = request.Idcustomermessage;
            entity.Customermessagetext = request.Customermessagetext;
            entity.IsCopy = request.IsCopy;
            entity.CopiedFromEstimateId = request.CopiedFromEstimateId;

            await _context.SaveChangesAsync(cancellationToken);

            return new Result<Domain.Entities.Estimate>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(UpdateEstimateCommandHandler));
            return new Result<Domain.Entities.Estimate>(ex);
        }
    }
}