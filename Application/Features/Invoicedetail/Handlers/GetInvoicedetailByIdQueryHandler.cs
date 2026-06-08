using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Invoicedetails;

public class GetInvoicedetailByIdQueryHandler : IRequestHandler<GetInvoicedetailByIdQuery, Result<Invoicedetail?>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetInvoicedetailByIdQueryHandler> _logger;

    public GetInvoicedetailByIdQueryHandler(AppDbContext context, ILogger<GetInvoicedetailByIdQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Invoicedetail?>> Handle(GetInvoicedetailByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetInvoicedetailByIdQueryHandler));

        try
        {
            _logger.LogInformation("Getting {EntityName} by id {Id}. Handler: {HandlerName}.", "InvoicedetailById", request.Id, nameof(GetInvoicedetailByIdQueryHandler));
            var entity = await _context.Invoicedetails.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity is null)
            {
                var notFoundException = new KeyNotFoundException("Invoicedetail not found");
                _logger.LogWarning(notFoundException, "{EntityName} with id {Id} was not found. Handler: {HandlerName}.", "Invoicedetail", request.Id, nameof(GetInvoicedetailByIdQueryHandler));
                return new Result<Invoicedetail?>(notFoundException);
            }

            return new Result<Invoicedetail?>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetInvoicedetailByIdQueryHandler));
            return new Result<Invoicedetail?>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetInvoicedetailByIdQueryHandler));
        }
    }
}