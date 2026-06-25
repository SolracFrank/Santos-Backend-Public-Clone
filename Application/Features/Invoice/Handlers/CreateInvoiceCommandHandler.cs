using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Invoices;

public class CreateInvoiceCommandHandler
    : IRequestHandler<CreateInvoiceCommand, Result<Domain.Entities.Invoice>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateInvoiceCommandHandler> _logger;

    public CreateInvoiceCommandHandler(AppDbContext context, ILogger<CreateInvoiceCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Domain.Entities.Invoice>> Handle(
        CreateInvoiceCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (request.Createdfromestimateid != null)
            {
                var estimate = await _context.Set<Domain.Entities.Estimate>()
                    .FirstOrDefaultAsync(x =>
                        x.Idestimate == request.Createdfromestimateid,
                        cancellationToken);

                if (estimate == null)
                {
                    return new Result<Domain.Entities.Invoice>(
                        new InvalidOperationException("The estimate does not exist."));
                }
            }

            const int maxIntentos = 5;

            for (int intento = 1; intento <= maxIntentos; intento++)
            {
                int invoiceNumber = await ObtenerNumeroSeguroAsync(
                    request.Invoicenumber,
                    request.Createdfromestimateid,
                    cancellationToken);

                var entity = new Domain.Entities.Invoice
                {
                    Idinvoice = request.Idinvoice,
                    Invoicenumber = invoiceNumber,
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

                _context.Set<Domain.Entities.Invoice>().Add(entity);

                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    return new Result<Domain.Entities.Invoice>(entity);
                }
                catch (DbUpdateException ex)
                {
                    _context.Entry(entity).State = EntityState.Detached;

                    _logger.LogWarning(
                        ex,
                        "Invoice number conflict. Retrying. Attempt {Attempt}.",
                        intento);

                    if (intento == maxIntentos)
                        return new Result<Domain.Entities.Invoice>(ex);
                }
            }

            return new Result<Domain.Entities.Invoice>(
                new InvalidOperationException("Could not create invoice after several attempts."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(CreateInvoiceCommandHandler));
            return new Result<Domain.Entities.Invoice>(ex);
        }
    }

    private async Task<int> ObtenerNumeroSeguroAsync(
        int numeroSolicitado,
        int? createdFromEstimateId,
        CancellationToken cancellationToken)
    {
        if (createdFromEstimateId.HasValue)
            return numeroSolicitado;

        if (numeroSolicitado > 0)
        {
            bool exists = await _context.Set<Domain.Entities.Invoice>()
                .AnyAsync(x => x.Invoicenumber == numeroSolicitado, cancellationToken);

            if (!exists)
                return numeroSolicitado;
        }

        int lastNumber = await _context.Set<Domain.Entities.Invoice>()
            .MaxAsync(x => (int?)x.Invoicenumber, cancellationToken) ?? 0;

        return lastNumber + 1;
    }
}