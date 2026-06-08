using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LanguageExt.Common;

namespace Application.Features.Customermessages;

public class UpdateCustomermessageCommandHandler : IRequestHandler<UpdateCustomermessageCommand, Result<Customermessage>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<UpdateCustomermessageCommandHandler> _logger;

    public UpdateCustomermessageCommandHandler(AppDbContext context, ILogger<UpdateCustomermessageCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Customermessage>> Handle(UpdateCustomermessageCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(UpdateCustomermessageCommandHandler));

        try
        {
            if (request.Id != request.Idcustomermessage)
                throw new ArgumentException("Id mismatch");

            var entity = new Customermessage
            {
                Idcustomermessage = request.Idcustomermessage,
                Message = request.Message
            };
            _logger.LogInformation("Updating {EntityName} with id {Id}. Handler: {HandlerName}.", "Customermessage", request.Id, nameof(UpdateCustomermessageCommandHandler));
            _context.Entry(entity).State = EntityState.Modified;
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(UpdateCustomermessageCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling UpdateCustomermessageCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(UpdateCustomermessageCommandHandler));
                return new Result<Customermessage>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(UpdateCustomermessageCommandHandler));
            return new Result<Customermessage>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(UpdateCustomermessageCommandHandler));
            return new Result<Customermessage>(ex);
        }
    }
}