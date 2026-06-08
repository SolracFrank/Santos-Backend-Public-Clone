using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using LanguageExt.Common;

namespace Application.Features.Repsales;

public class CreateRepsaleCommandHandler : IRequestHandler<CreateRepsaleCommand, Result<Repsale>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateRepsaleCommandHandler> _logger;

    public CreateRepsaleCommandHandler(AppDbContext context, ILogger<CreateRepsaleCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Repsale>> Handle(CreateRepsaleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(CreateRepsaleCommandHandler));

        try
        {
            var entity = new Repsale
            {
                Idrepsales = request.Idrepsales,
                Rinitials = request.Rinitials,
                Idemployee = request.Idemployee,
                Idothern = request.Idothern
            };
            _logger.LogInformation("Creating {EntityName}. Handler: {HandlerName}.", nameof(Repsale), nameof(CreateRepsaleCommandHandler));
            _context.Set<Repsale>().Add(entity);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(CreateRepsaleCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling CreateRepsaleCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(CreateRepsaleCommandHandler));
                return new Result<Repsale>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(CreateRepsaleCommandHandler));
            return new Result<Repsale>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(CreateRepsaleCommandHandler));
            return new Result<Repsale>(ex);
        }
    }
}