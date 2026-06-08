using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Estates;

public class GetEstatesQueryHandler : IRequestHandler<GetEstatesQuery, Result<List<Estate>>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetEstatesQueryHandler> _logger;

    public GetEstatesQueryHandler(AppDbContext context, ILogger<GetEstatesQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<List<Estate>>> Handle(GetEstatesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetEstatesQueryHandler));

        try
        {
            _logger.LogInformation("Getting list for {EntityName}. Handler: {HandlerName}.", "Estate", nameof(GetEstatesQueryHandler));
            return new Result<List<Estate>>(await _context.Estates.ToListAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetEstatesQueryHandler));
            return new Result<List<Estate>>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetEstatesQueryHandler));
        }
    }
}