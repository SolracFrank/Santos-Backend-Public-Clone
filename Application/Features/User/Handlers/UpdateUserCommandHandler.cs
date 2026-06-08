using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LanguageExt.Common;

namespace Application.Features.Users;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<User>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<UpdateUserCommandHandler> _logger;

    public UpdateUserCommandHandler(AppDbContext context, ILogger<UpdateUserCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<User>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(UpdateUserCommandHandler));

        try
        {
            if (request.Id != request.Idusers)
                throw new ArgumentException("Id mismatch");

            var entity = new User
            {
                Idusers = request.Idusers,
                Ucustomer = request.Ucustomer,
                Ubalance = request.Ubalance,
                Udate = request.Udate,
                Idaddress = request.Idaddress,
                Idterms = request.Idterms,
                Idcustomer = request.Idcustomer,
                Idrepsales = request.Idrepsales,
                Idjobinfo = request.Idjobinfo
            };
            _logger.LogInformation("Updating {EntityName} with id {Id}. Handler: {HandlerName}.", "User", request.Id, nameof(UpdateUserCommandHandler));
            _context.Entry(entity).State = EntityState.Modified;
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(UpdateUserCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling UpdateUserCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(UpdateUserCommandHandler));
                return new Result<User>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(UpdateUserCommandHandler));
            return new Result<User>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(UpdateUserCommandHandler));
            return new Result<User>(ex);
        }
    }
}