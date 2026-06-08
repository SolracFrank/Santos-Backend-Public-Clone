using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LanguageExt.Common;

namespace Application.Features.Repsales;

public class UpdateRepsaleCommandHandler : IRequestHandler<UpdateRepsaleCommand, Result<Repsale>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<UpdateRepsaleCommandHandler> _logger;

    public UpdateRepsaleCommandHandler(AppDbContext context, ILogger<UpdateRepsaleCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Repsale>> Handle(UpdateRepsaleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(UpdateRepsaleCommandHandler));

        try
        {
            if (request.Id != request.Idrepsales)
                throw new ArgumentException("Id mismatch");

            var entity = new Repsale
            {
                Idrepsales = request.Idrepsales,
                Rinitials = request.Rinitials,
                Idemployee = request.Idemployee,
                Idothern = request.Idothern
            };
            _logger.LogInformation("Updating {EntityName} with id {Id}. Handler: {HandlerName}.", "Repsale", request.Id, nameof(UpdateRepsaleCommandHandler));
            _context.Entry(entity).State = EntityState.Modified;
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(UpdateRepsaleCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling UpdateRepsaleCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(UpdateRepsaleCommandHandler));
                return new Result<Repsale>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(UpdateRepsaleCommandHandler));
            return new Result<Repsale>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(UpdateRepsaleCommandHandler));
            return new Result<Repsale>(ex);
        }
    }
}