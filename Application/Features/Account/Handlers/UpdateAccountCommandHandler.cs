using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LanguageExt.Common;

namespace Application.Features.Accounts;

public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, Result<Account>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<UpdateAccountCommandHandler> _logger;

    public UpdateAccountCommandHandler(AppDbContext context, ILogger<UpdateAccountCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Account>> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(UpdateAccountCommandHandler));

        try
        {
            if (request.Id != request.Idaccount)
                throw new ArgumentException("Id mismatch");

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
            _logger.LogInformation("Updating {EntityName} with id {Id}. Handler: {HandlerName}.", "Account", request.Id, nameof(UpdateAccountCommandHandler));
            _context.Entry(entity).State = EntityState.Modified;
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(UpdateAccountCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling UpdateAccountCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(UpdateAccountCommandHandler));
                return new Result<Account>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(UpdateAccountCommandHandler));
            return new Result<Account>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(UpdateAccountCommandHandler));
            return new Result<Account>(ex);
        }
    }
}