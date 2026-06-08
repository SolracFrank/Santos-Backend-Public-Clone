using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Invoicedetails;

public class DeleteInvoicedetailsByInvoiceCommandHandler : IRequestHandler<DeleteInvoicedetailsByInvoiceCommand, Result<Unit>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<DeleteInvoicedetailsByInvoiceCommandHandler> _logger;

    public DeleteInvoicedetailsByInvoiceCommandHandler(AppDbContext context, ILogger<DeleteInvoicedetailsByInvoiceCommandHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Unit>> Handle(DeleteInvoicedetailsByInvoiceCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(DeleteInvoicedetailsByInvoiceCommandHandler));

        try
        {
            var items = await _context.Invoicedetails
                .Where(x => x.Idinvoice == request.IdInvoice)
                .ToListAsync(cancellationToken);
        
            _context.Invoicedetails.RemoveRange(items);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(DeleteInvoicedetailsByInvoiceCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling DeleteInvoicedetailsByInvoiceCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(DeleteInvoicedetailsByInvoiceCommandHandler));
                return new Result<Unit>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(DeleteInvoicedetailsByInvoiceCommandHandler));
            return new Result<Unit>(Unit.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(DeleteInvoicedetailsByInvoiceCommandHandler));
            return new Result<Unit>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(DeleteInvoicedetailsByInvoiceCommandHandler));
        }
    }
}