using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Itemtypes;

public class GetItemtypesQueryHandler : IRequestHandler<GetItemtypesQuery, Result<List<Itemtype>>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetItemtypesQueryHandler> _logger;

    public GetItemtypesQueryHandler(AppDbContext context, ILogger<GetItemtypesQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<List<Itemtype>>> Handle(GetItemtypesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetItemtypesQueryHandler));

        try
        {
            _logger.LogInformation("Getting list for {EntityName}. Handler: {HandlerName}.", "Itemtype", nameof(GetItemtypesQueryHandler));
            return new Result<List<Itemtype>>(await _context.Itemtypes.ToListAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetItemtypesQueryHandler));
            return new Result<List<Itemtype>>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetItemtypesQueryHandler));
        }
    }
}