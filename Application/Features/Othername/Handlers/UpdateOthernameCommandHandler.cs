using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LanguageExt.Common;

namespace Application.Features.Othernames;

public class UpdateOthernameCommandHandler : IRequestHandler<UpdateOthernameCommand, Result<Othername>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<UpdateOthernameCommandHandler> _logger;

    public UpdateOthernameCommandHandler(AppDbContext context, ILogger<UpdateOthernameCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Othername>> Handle(UpdateOthernameCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(UpdateOthernameCommandHandler));

        try
        {
            if (request.Id != request.Idother)
                throw new ArgumentException("Id mismatch");

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
            _logger.LogInformation("Updating {EntityName} with id {Id}. Handler: {HandlerName}.", "Othername", request.Id, nameof(UpdateOthernameCommandHandler));
            _context.Entry(entity).State = EntityState.Modified;
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(UpdateOthernameCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling UpdateOthernameCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(UpdateOthernameCommandHandler));
                return new Result<Othername>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(UpdateOthernameCommandHandler));
            return new Result<Othername>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(UpdateOthernameCommandHandler));
            return new Result<Othername>(ex);
        }
    }
}