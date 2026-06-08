using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Users;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, Result<List<User>>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetUsersQueryHandler> _logger;

    public GetUsersQueryHandler(AppDbContext context, ILogger<GetUsersQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<List<User>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetUsersQueryHandler));

        try
        {
            _logger.LogInformation("Getting list for {EntityName}. Handler: {HandlerName}.", "User", nameof(GetUsersQueryHandler));
            return new Result<List<User>>(await _context.Users.ToListAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetUsersQueryHandler));
            return new Result<List<User>>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetUsersQueryHandler));
        }
    }
}