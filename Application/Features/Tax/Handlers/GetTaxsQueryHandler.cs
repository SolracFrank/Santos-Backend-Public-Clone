using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Taxes;

public class GetTaxsQueryHandler : IRequestHandler<GetTaxsQuery, Result<List<Tax>>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetTaxsQueryHandler> _logger;

    public GetTaxsQueryHandler(AppDbContext context, ILogger<GetTaxsQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<List<Tax>>> Handle(GetTaxsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetTaxsQueryHandler));

        try
        {
            _logger.LogInformation("Getting list for {EntityName}. Handler: {HandlerName}.", "Tax", nameof(GetTaxsQueryHandler));
            return new Result<List<Tax>>(await _context.Taxes.ToListAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetTaxsQueryHandler));
            return new Result<List<Tax>>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetTaxsQueryHandler));
        }
    }
}