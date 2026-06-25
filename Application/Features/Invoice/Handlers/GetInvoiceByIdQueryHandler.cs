using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Invoices;

public class GetInvoiceByIdQueryHandler : IRequestHandler<GetInvoiceByIdQuery, Result<Domain.Entities.Invoice?>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetInvoiceByIdQueryHandler> _logger;

    public GetInvoiceByIdQueryHandler(AppDbContext context, ILogger<GetInvoiceByIdQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Domain.Entities.Invoice?>> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetInvoiceByIdQueryHandler));

        try
        {
            _logger.LogInformation("Getting {EntityName} by id {Id}. Handler: {HandlerName}.", "InvoiceById", request.Id, nameof(GetInvoiceByIdQueryHandler));
            var entity = await _context.Invoices.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity is null)
            {
                var notFoundException = new KeyNotFoundException("Invoice not found");
                _logger.LogWarning(notFoundException, "{EntityName} with id {Id} was not found. Handler: {HandlerName}.", "Invoice", request.Id, nameof(GetInvoiceByIdQueryHandler));
                return new Result<Domain.Entities.Invoice?>(notFoundException);
            }

            return new Result<Domain.Entities.Invoice?>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetInvoiceByIdQueryHandler));
            return new Result<Domain.Entities.Invoice?>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetInvoiceByIdQueryHandler));
        }
    }
}