using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Itservices;

public class GetItservicesQueryHandler : IRequestHandler<GetItservicesQuery, Result<List<Itservice>>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetItservicesQueryHandler> _logger;

    public GetItservicesQueryHandler(AppDbContext context, ILogger<GetItservicesQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<List<Itservice>>> Handle(GetItservicesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetItservicesQueryHandler));

        try
        {
            _logger.LogInformation("Getting list for {EntityName}. Handler: {HandlerName}.", "Itservice", nameof(GetItservicesQueryHandler));
            return new Result<List<Itservice>>(await _context.Itservices.ToListAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetItservicesQueryHandler));
            return new Result<List<Itservice>>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetItservicesQueryHandler));
        }
    }
}