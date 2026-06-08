using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using LanguageExt.Common;

namespace Application.Features.Customermessages;

public class CreateCustomermessageCommandHandler : IRequestHandler<CreateCustomermessageCommand, Result<Customermessage>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateCustomermessageCommandHandler> _logger;

    public CreateCustomermessageCommandHandler(AppDbContext context, ILogger<CreateCustomermessageCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Customermessage>> Handle(CreateCustomermessageCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(CreateCustomermessageCommandHandler));

        try
        {
            var entity = new Customermessage
            {
                Idcustomermessage = request.Idcustomermessage,
                Message = request.Message
            };
            _logger.LogInformation("Creating {EntityName}. Handler: {HandlerName}.", nameof(Customermessage), nameof(CreateCustomermessageCommandHandler));
            _context.Set<Customermessage>().Add(entity);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(CreateCustomermessageCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling CreateCustomermessageCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(CreateCustomermessageCommandHandler));
                return new Result<Customermessage>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(CreateCustomermessageCommandHandler));
            return new Result<Customermessage>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(CreateCustomermessageCommandHandler));
            return new Result<Customermessage>(ex);
        }
    }
}