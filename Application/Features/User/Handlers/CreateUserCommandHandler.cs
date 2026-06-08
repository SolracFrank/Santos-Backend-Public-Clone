using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using LanguageExt.Common;

namespace Application.Features.Users;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<User>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateUserCommandHandler> _logger;

    public CreateUserCommandHandler(AppDbContext context, ILogger<CreateUserCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<User>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(CreateUserCommandHandler));

        try
        {
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
            _logger.LogInformation("Creating {EntityName}. Handler: {HandlerName}.", nameof(User), nameof(CreateUserCommandHandler));
            _context.Set<User>().Add(entity);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(CreateUserCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling CreateUserCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(CreateUserCommandHandler));
                return new Result<User>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(CreateUserCommandHandler));
            return new Result<User>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(CreateUserCommandHandler));
            return new Result<User>(ex);
        }
    }
}