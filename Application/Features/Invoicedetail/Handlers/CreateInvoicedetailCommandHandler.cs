using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using LanguageExt.Common;

namespace Application.Features.Invoicedetails;

public class CreateInvoicedetailCommandHandler : IRequestHandler<CreateInvoicedetailCommand, Result<Invoicedetail>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateInvoicedetailCommandHandler> _logger;

    public CreateInvoicedetailCommandHandler(AppDbContext context, ILogger<CreateInvoicedetailCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Invoicedetail>> Handle(CreateInvoicedetailCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(CreateInvoicedetailCommandHandler));

        try
        {
            var entity = new Invoicedetail
            {
                Idinvoicedetail = request.Idinvoicedetail,
                Idinvoice = request.Idinvoice,
                Iditem = request.Iditem,
                Itemname = request.Itemname,
                Description = request.Description,
                Qty = request.Qty,
                Cost = request.Cost,
                Total = request.Total
            };
            _logger.LogInformation("Creating {EntityName}. Handler: {HandlerName}.", nameof(Invoicedetail), nameof(CreateInvoicedetailCommandHandler));
            _context.Set<Invoicedetail>().Add(entity);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(CreateInvoicedetailCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling CreateInvoicedetailCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(CreateInvoicedetailCommandHandler));
                return new Result<Invoicedetail>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(CreateInvoicedetailCommandHandler));
            return new Result<Invoicedetail>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(CreateInvoicedetailCommandHandler));
            return new Result<Invoicedetail>(ex);
        }
    }
}