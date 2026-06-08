using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LanguageExt.Common;

namespace Application.Features.Invoicedetails;

public class UpdateInvoicedetailCommandHandler : IRequestHandler<UpdateInvoicedetailCommand, Result<Invoicedetail>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<UpdateInvoicedetailCommandHandler> _logger;

    public UpdateInvoicedetailCommandHandler(AppDbContext context, ILogger<UpdateInvoicedetailCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Invoicedetail>> Handle(UpdateInvoicedetailCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(UpdateInvoicedetailCommandHandler));

        try
        {
            if (request.Id != request.Idinvoicedetail)
                throw new ArgumentException("Id mismatch");

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
            _logger.LogInformation("Updating {EntityName} with id {Id}. Handler: {HandlerName}.", "Invoicedetail", request.Id, nameof(UpdateInvoicedetailCommandHandler));
            _context.Entry(entity).State = EntityState.Modified;
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(UpdateInvoicedetailCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling UpdateInvoicedetailCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(UpdateInvoicedetailCommandHandler));
                return new Result<Invoicedetail>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(UpdateInvoicedetailCommandHandler));
            return new Result<Invoicedetail>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(UpdateInvoicedetailCommandHandler));
            return new Result<Invoicedetail>(ex);
        }
    }
}