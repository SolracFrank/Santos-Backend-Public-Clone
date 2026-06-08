using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using LanguageExt.Common;

namespace Application.Features.Invoicedetails;

public class DeleteInvoicedetailCommandHandler : IRequestHandler<DeleteInvoicedetailCommand, Result<Unit>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<DeleteInvoicedetailCommandHandler> _logger;

    public DeleteInvoicedetailCommandHandler(AppDbContext context, ILogger<DeleteInvoicedetailCommandHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Unit>> Handle(DeleteInvoicedetailCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(DeleteInvoicedetailCommandHandler));

        try
        {
            var item = await _context.Invoicedetails.FindAsync(new object[] { request.Id }, cancellationToken);
            if (item == null)
            {
                _logger.LogWarning("Invoicedetail not found. Handler: {HandlerName}.", nameof(DeleteInvoicedetailCommandHandler));
                throw new KeyNotFoundException("Invoicedetail not found");
            }
            _logger.LogInformation("Deleting {EntityName} with id {Id}. Handler: {HandlerName}.", "Invoicedetail", request.Id, nameof(DeleteInvoicedetailCommandHandler));
            _context.Invoicedetails.Remove(item);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(DeleteInvoicedetailCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling DeleteInvoicedetailCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(DeleteInvoicedetailCommandHandler));
                return new Result<Unit>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(DeleteInvoicedetailCommandHandler));
            return new Result<Unit>(Unit.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(DeleteInvoicedetailCommandHandler));
            return new Result<Unit>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(DeleteInvoicedetailCommandHandler));
        }
    }
}