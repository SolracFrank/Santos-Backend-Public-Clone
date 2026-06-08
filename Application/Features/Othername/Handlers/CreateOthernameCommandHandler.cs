using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using LanguageExt.Common;

namespace Application.Features.Othernames;

public class CreateOthernameCommandHandler : IRequestHandler<CreateOthernameCommand, Result<Othername>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateOthernameCommandHandler> _logger;

    public CreateOthernameCommandHandler(AppDbContext context, ILogger<CreateOthernameCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Othername>> Handle(CreateOthernameCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(CreateOthernameCommandHandler));

        try
        {
            var entity = new Othername
            {
                Idother = request.Idother,
                Oname = request.Oname,
                Olastname = request.Olastname,
                Oaddress = request.Oaddress,
                Ocity = request.Ocity,
                Ophone = request.Ophone,
                Oemail = request.Oemail,
                Ozip = request.Ozip,
                Idstate = request.Idstate
            };
            _logger.LogInformation("Creating {EntityName}. Handler: {HandlerName}.", nameof(Othername), nameof(CreateOthernameCommandHandler));
            _context.Set<Othername>().Add(entity);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(CreateOthernameCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling CreateOthernameCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(CreateOthernameCommandHandler));
                return new Result<Othername>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(CreateOthernameCommandHandler));
            return new Result<Othername>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(CreateOthernameCommandHandler));
            return new Result<Othername>(ex);
        }
    }
}