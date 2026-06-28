using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using LanguageExt.Common;
using Domain.Common.Audit;
using Domain.Common.Security;
using Domain.Entities.Dtos;

namespace Application.Features.Invoices;

public class DeleteInvoiceCommandHandler : IRequestHandler<DeleteInvoiceCommand, Result<Unit>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<DeleteInvoiceCommandHandler> _logger;
    private readonly IAuditService _auditService;
    private readonly ICurrentUserService _currentUserService;

    public DeleteInvoiceCommandHandler(
    AppDbContext context,
    ILogger<DeleteInvoiceCommandHandler> logger,
    IAuditService auditService,
    ICurrentUserService currentUserService)
    {
        _logger = logger;
        _context = context;
        _auditService = auditService;
        _currentUserService = currentUserService;
    }

    public async Task<Result<Unit>> Handle(DeleteInvoiceCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(DeleteInvoiceCommandHandler));

        try
        {
            var item = await _context.Invoices.FindAsync(new object[] { request.Id }, cancellationToken);
            if (item == null)
            {
                _logger.LogWarning("Invoice not found. Handler: {HandlerName}.", nameof(DeleteInvoiceCommandHandler));
                throw new KeyNotFoundException("Invoice not found");
            }
            _logger.LogInformation("Hiding {EntityName} with id {Id}. Handler: {HandlerName}.", "Invoice", request.Id, nameof(DeleteInvoiceCommandHandler));

            item.Activeinvoice = false;
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(DeleteInvoiceCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling DeleteInvoiceCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(DeleteInvoiceCommandHandler));
                return new Result<Unit>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(DeleteInvoiceCommandHandler));

            await _auditService.LogAsync(new AuditLogDto
            {
                IdLoginUser = _currentUserService.IdLoginUser,
                Username = _currentUserService.Username,
                Action = AuditActions.InvoiceDeleted,
                EntityName = "Invoices",
                EntityId = item.Idinvoice.ToString(),
                Description = $"Deleted invoice #{item.Invoicenumber}.",
                PerformedByRole = _currentUserService.UserRole,
                Succeeded = true,
                Severity = AuditSeverity.Warning
            });

            return new Result<Unit>(Unit.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(DeleteInvoiceCommandHandler));
            return new Result<Unit>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(DeleteInvoiceCommandHandler));
        }
    }
}