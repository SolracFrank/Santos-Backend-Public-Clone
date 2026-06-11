using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;

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
            bool existeInvoice = await _context.Invoices
            .AnyAsync(x => x.Invoicenumber == request.Invoicenumber, cancellationToken);

            if (existeInvoice)
            {
                return new Result<Invoice>(
                    new InvalidOperationException(
                        $"Invoice #{request.Invoicenumber} already exists."));
            }

            if (request.Createdfromestimateid != null)
            {
                var estimate = await _context.Estimates
                    .FirstOrDefaultAsync(x => x.Idestimate == request.Createdfromestimateid, cancellationToken);

                if (estimate == null)
                {
                    return new Result<Invoice>(
                        new InvalidOperationException("The estimate does not exist."));
                }

                if (estimate.Estimatenumber != request.Invoicenumber)
                {
                    return new Result<Invoice>(
                        new InvalidOperationException(
                            $"Invoice number must match Estimate number #{estimate.Estimatenumber}."));
                }
            }

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