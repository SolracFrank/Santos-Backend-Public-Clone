using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Payts;

public class GetPaytsQueryHandler : IRequestHandler<GetPaytsQuery, Result<List<Payt>>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetPaytsQueryHandler> _logger;

    public GetPaytsQueryHandler(AppDbContext context, ILogger<GetPaytsQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<List<Payt>>> Handle(GetPaytsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetPaytsQueryHandler));

        try
        {
            _logger.LogInformation("Getting list for {EntityName}. Handler: {HandlerName}.", "Payt", nameof(GetPaytsQueryHandler));
            return new Result<List<Payt>>(await _context.Payts.ToListAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetPaytsQueryHandler));
            return new Result<List<Payt>>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetPaytsQueryHandler));
        }
    }
}