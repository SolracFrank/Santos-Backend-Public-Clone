using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Invoices;

public class GetInvoicesQueryHandler : IRequestHandler<GetInvoicesQuery, Result<List<Domain.Entities.Invoice>>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetInvoicesQueryHandler> _logger;

    public GetInvoicesQueryHandler(AppDbContext context, ILogger<GetInvoicesQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<List<Domain.Entities.Invoice>>> Handle(GetInvoicesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetInvoicesQueryHandler));

        try
        {
            _logger.LogInformation("Getting list for {EntityName}. Handler: {HandlerName}.", "Invoice", nameof(GetInvoicesQueryHandler));
            return new Result<List<Domain.Entities.Invoice>>(await _context.Invoices.ToListAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetInvoicesQueryHandler));
            return new Result<List<Domain.Entities.Invoice>>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetInvoicesQueryHandler));
        }
    }
}