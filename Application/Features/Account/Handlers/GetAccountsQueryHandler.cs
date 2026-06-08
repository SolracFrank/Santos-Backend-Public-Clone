using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Accounts;

public class GetAccountsQueryHandler : IRequestHandler<GetAccountsQuery, Result<List<Account>>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetAccountsQueryHandler> _logger;

    public GetAccountsQueryHandler(AppDbContext context, ILogger<GetAccountsQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<List<Account>>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetAccountsQueryHandler));

        try
        {
            _logger.LogInformation("Getting list for {EntityName}. Handler: {HandlerName}.", "Account", nameof(GetAccountsQueryHandler));
            return new Result<List<Account>>(await _context.Accounts.ToListAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetAccountsQueryHandler));
            return new Result<List<Account>>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetAccountsQueryHandler));
        }
    }
}