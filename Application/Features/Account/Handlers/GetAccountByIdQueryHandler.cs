using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Accounts;

public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, Result<Account?>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetAccountByIdQueryHandler> _logger;

    public GetAccountByIdQueryHandler(AppDbContext context, ILogger<GetAccountByIdQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Account?>> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetAccountByIdQueryHandler));

        try
        {
            _logger.LogInformation("Getting {EntityName} by id {Id}. Handler: {HandlerName}.", "AccountById", request.Id, nameof(GetAccountByIdQueryHandler));
            var entity = await _context.Accounts.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity is null)
            {
                var notFoundException = new KeyNotFoundException("Account not found");
                _logger.LogWarning(notFoundException, "{EntityName} with id {Id} was not found. Handler: {HandlerName}.", "Account", request.Id, nameof(GetAccountByIdQueryHandler));
                return new Result<Account?>(notFoundException);
            }

            return new Result<Account?>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetAccountByIdQueryHandler));
            return new Result<Account?>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetAccountByIdQueryHandler));
        }
    }
}