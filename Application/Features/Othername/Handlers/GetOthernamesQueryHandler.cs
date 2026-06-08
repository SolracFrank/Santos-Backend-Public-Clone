using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Othernames;

public class GetOthernamesQueryHandler : IRequestHandler<GetOthernamesQuery, Result<List<Othername>>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetOthernamesQueryHandler> _logger;

    public GetOthernamesQueryHandler(AppDbContext context, ILogger<GetOthernamesQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<List<Othername>>> Handle(GetOthernamesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetOthernamesQueryHandler));

        try
        {
            _logger.LogInformation("Getting list for {EntityName}. Handler: {HandlerName}.", "Othername", nameof(GetOthernamesQueryHandler));
            return new Result<List<Othername>>(await _context.Othernames.ToListAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetOthernamesQueryHandler));
            return new Result<List<Othername>>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetOthernamesQueryHandler));
        }
    }
}