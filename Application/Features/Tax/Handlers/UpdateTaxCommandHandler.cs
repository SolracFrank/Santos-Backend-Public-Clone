using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LanguageExt.Common;

namespace Application.Features.Taxes;

public class UpdateTaxCommandHandler : IRequestHandler<UpdateTaxCommand, Result<Tax>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<UpdateTaxCommandHandler> _logger;

    public UpdateTaxCommandHandler(AppDbContext context, ILogger<UpdateTaxCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Tax>> Handle(UpdateTaxCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(UpdateTaxCommandHandler));

        try
        {
            if (request.Id != request.Idtax)
                throw new ArgumentException("Id mismatch");

            var entity = new Tax
            {
                Idtax = request.Idtax,
                Tname = request.Tname
            };
            _logger.LogInformation("Updating {EntityName} with id {Id}. Handler: {HandlerName}.", "Tax", request.Id, nameof(UpdateTaxCommandHandler));
            _context.Entry(entity).State = EntityState.Modified;
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(UpdateTaxCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling UpdateTaxCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(UpdateTaxCommandHandler));
                return new Result<Tax>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(UpdateTaxCommandHandler));
            return new Result<Tax>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(UpdateTaxCommandHandler));
            return new Result<Tax>(ex);
        }
    }
}