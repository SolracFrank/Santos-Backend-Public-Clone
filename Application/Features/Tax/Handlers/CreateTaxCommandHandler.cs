using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using LanguageExt.Common;

namespace Application.Features.Taxes;

public class CreateTaxCommandHandler : IRequestHandler<CreateTaxCommand, Result<Tax>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateTaxCommandHandler> _logger;

    public CreateTaxCommandHandler(AppDbContext context, ILogger<CreateTaxCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Tax>> Handle(CreateTaxCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(CreateTaxCommandHandler));

        try
        {
            var entity = new Tax
            {
                Idtax = request.Idtax,
                Tname = request.Tname
            };
            _logger.LogInformation("Creating {EntityName}. Handler: {HandlerName}.", nameof(Tax), nameof(CreateTaxCommandHandler));
            _context.Set<Tax>().Add(entity);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(CreateTaxCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling CreateTaxCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(CreateTaxCommandHandler));
                return new Result<Tax>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(CreateTaxCommandHandler));
            return new Result<Tax>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(CreateTaxCommandHandler));
            return new Result<Tax>(ex);
        }
    }
}