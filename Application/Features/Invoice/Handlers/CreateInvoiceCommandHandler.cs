using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using LanguageExt.Common;

namespace Application.Features.Invoices;

public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, Result<Invoice>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateInvoiceCommandHandler> _logger;

    public CreateInvoiceCommandHandler(AppDbContext context, ILogger<CreateInvoiceCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Invoice>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(CreateInvoiceCommandHandler));

        try
        {
            var entity = new Invoice
            {
                Idinvoice = request.Idinvoice,
                Invoicenumber = request.Invoicenumber,
                Invoicedate = request.Invoicedate,
                Duedate = request.Duedate,
                Dteordered = request.Dteordered,
                Ponumber = request.Ponumber,
                Memo = request.Memo,
                Subtotal = request.Subtotal,
                Markup = request.Markup,
                Total = request.Total,
                Ppenbalance = request.Ppenbalance,
                Activeinvoice = request.Activeinvoice,
                Idusers = request.Idusers,
                Idterms = request.Idterms,
                Idrepsales = request.Idrepsales,
                Idcustomermessage = request.Idcustomermessage,
                Customermessagetext = request.Customermessagetext,
                Createdfromestimateid = request.Createdfromestimateid,
                Dateordered = request.Dateordered,
                Openbalance = request.Openbalance
            };
            _logger.LogInformation("Creating {EntityName}. Handler: {HandlerName}.", nameof(Invoice), nameof(CreateInvoiceCommandHandler));
            _context.Set<Invoice>().Add(entity);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(CreateInvoiceCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling CreateInvoiceCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(CreateInvoiceCommandHandler));
                return new Result<Invoice>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(CreateInvoiceCommandHandler));
            return new Result<Invoice>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(CreateInvoiceCommandHandler));
            return new Result<Invoice>(ex);
        }
    }
}