using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LanguageExt.Common;

namespace Application.Features.Estates;

public class UpdateEstateCommandHandler : IRequestHandler<UpdateEstateCommand, Result<Estate>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<UpdateEstateCommandHandler> _logger;

    public UpdateEstateCommandHandler(AppDbContext context, ILogger<UpdateEstateCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Estate>> Handle(UpdateEstateCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(UpdateEstateCommandHandler));

        try
        {
            if (request.Id != request.Idestate)
                throw new ArgumentException("Id mismatch");

            var entity = new Estate
            {
                Idestate = request.Idestate,
                Esname = request.Esname
            };
            _logger.LogInformation("Updating {EntityName} with id {Id}. Handler: {HandlerName}.", "Estate", request.Id, nameof(UpdateEstateCommandHandler));
            _context.Entry(entity).State = EntityState.Modified;
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(UpdateEstateCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling UpdateEstateCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(UpdateEstateCommandHandler));
                return new Result<Estate>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(UpdateEstateCommandHandler));
            return new Result<Estate>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(UpdateEstateCommandHandler));
            return new Result<Estate>(ex);
        }
    }
}