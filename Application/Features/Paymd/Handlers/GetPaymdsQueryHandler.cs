using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Paymds;

public class GetPaymdsQueryHandler : IRequestHandler<GetPaymdsQuery, Result<List<Paymd>>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetPaymdsQueryHandler> _logger;

    public GetPaymdsQueryHandler(AppDbContext context, ILogger<GetPaymdsQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<List<Paymd>>> Handle(GetPaymdsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetPaymdsQueryHandler));

        try
        {
            _logger.LogInformation("Getting list for {EntityName}. Handler: {HandlerName}.", "Paymd", nameof(GetPaymdsQueryHandler));
            return new Result<List<Paymd>>(await _context.Paymds.ToListAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetPaymdsQueryHandler));
            return new Result<List<Paymd>>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetPaymdsQueryHandler));
        }
    }
}