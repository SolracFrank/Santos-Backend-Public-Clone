using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using LanguageExt.Common;

namespace Application.Features.Estates;

public class CreateEstateCommandHandler : IRequestHandler<CreateEstateCommand, Result<Estate>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateEstateCommandHandler> _logger;

    public CreateEstateCommandHandler(AppDbContext context, ILogger<CreateEstateCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Estate>> Handle(CreateEstateCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(CreateEstateCommandHandler));

        try
        {
            var entity = new Estate
            {
                Idestate = request.Idestate,
                Esname = request.Esname
            };
            _logger.LogInformation("Creating {EntityName}. Handler: {HandlerName}.", nameof(Estate), nameof(CreateEstateCommandHandler));
            _context.Set<Estate>().Add(entity);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(CreateEstateCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling CreateEstateCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(CreateEstateCommandHandler));
                return new Result<Estate>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(CreateEstateCommandHandler));
            return new Result<Estate>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(CreateEstateCommandHandler));
            return new Result<Estate>(ex);
        }
    }
}