using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LanguageExt.Common;

namespace Application.Features.Invoices;

public class UpdateInvoiceCommandHandler : IRequestHandler<UpdateInvoiceCommand, Result<Invoice>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<UpdateInvoiceCommandHandler> _logger;

    public UpdateInvoiceCommandHandler(AppDbContext context, ILogger<UpdateInvoiceCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Invoice>> Handle(UpdateInvoiceCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(UpdateInvoiceCommandHandler));

        try
        {
            if (request.Id != request.Idinvoice)
                throw new ArgumentException("Id mismatch");

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
                Activeinvoice = request.Activeinvoice,
                Idusers = request.Idusers,
                Idterms = request.Idterms,
                Idrepsales = request.Idrepsales,
                Idcustomermessage = request.Idcustomermessage,
                Customermessagetext = request.Customermessagetext,
                Createdfromestimateid = request.Createdfromestimateid,
                Dateordered = request.Dateordered,
                Openbalance = request.Openbalance,
                Idtaxrate = request.Idtaxrate,
                Taxrate = request.Taxrate,
                Taxamount = request.Taxamount
            };
            _logger.LogInformation("Updating {EntityName} with id {Id}. Handler: {HandlerName}.", "Invoice", request.Id, nameof(UpdateInvoiceCommandHandler));
            _context.Entry(entity).State = EntityState.Modified;
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(UpdateInvoiceCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling UpdateInvoiceCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(UpdateInvoiceCommandHandler));
                return new Result<Invoice>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(UpdateInvoiceCommandHandler));
            return new Result<Invoice>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(UpdateInvoiceCommandHandler));
            return new Result<Invoice>(ex);
        }
    }
}