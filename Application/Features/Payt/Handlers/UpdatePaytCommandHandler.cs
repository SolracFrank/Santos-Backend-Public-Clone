using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LanguageExt.Common;

namespace Application.Features.Payts;

public class UpdatePaytCommandHandler : IRequestHandler<UpdatePaytCommand, Result<Payt>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<UpdatePaytCommandHandler> _logger;

    public UpdatePaytCommandHandler(AppDbContext context, ILogger<UpdatePaytCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Payt>> Handle(UpdatePaytCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(UpdatePaytCommandHandler));

        try
        {
            if (request.Id != request.Idpayt)
                throw new ArgumentException("Id mismatch");

            var entity = new Payt
            {
                Idpayt = request.Idpayt,
                Ptname = request.Ptname
            };
            _logger.LogInformation("Updating {EntityName} with id {Id}. Handler: {HandlerName}.", "Payt", request.Id, nameof(UpdatePaytCommandHandler));
            _context.Entry(entity).State = EntityState.Modified;
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(UpdatePaytCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling UpdatePaytCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(UpdatePaytCommandHandler));
                return new Result<Payt>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(UpdatePaytCommandHandler));
            return new Result<Payt>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(UpdatePaytCommandHandler));
            return new Result<Payt>(ex);
        }
    }
}