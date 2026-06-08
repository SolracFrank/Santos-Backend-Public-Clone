using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using LanguageExt.Common;

namespace Application.Features.Accounts;

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Result<Account>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateAccountCommandHandler> _logger;

    public CreateAccountCommandHandler(AppDbContext context, ILogger<CreateAccountCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Account>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(CreateAccountCommandHandler));

        try
        {
            var entity = new Account
            {
                Idaccount = request.Idaccount,
                Aname = request.Aname,
                Adescription = request.Adescription,
                Abankno = request.Abankno,
                Anotes = request.Anotes,
                Idtax = request.Idtax,
                Idaccountt = request.Idaccountt
            };
            _logger.LogInformation("Creating {EntityName}. Handler: {HandlerName}.", nameof(Account), nameof(CreateAccountCommandHandler));
            _context.Set<Account>().Add(entity);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(CreateAccountCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling CreateAccountCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(CreateAccountCommandHandler));
                return new Result<Account>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(CreateAccountCommandHandler));
            return new Result<Account>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(CreateAccountCommandHandler));
            return new Result<Account>(ex);
        }
    }
}