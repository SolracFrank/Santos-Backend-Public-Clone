using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LanguageExt.Common;
using Domain.Common.Audit;
using Domain.Common.Security;
using Domain.Entities.Dtos;

namespace Application.Features.Invoices;

public class UpdateInvoiceCommandHandler : IRequestHandler<UpdateInvoiceCommand, Result<Domain.Entities.Invoice>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<UpdateInvoiceCommandHandler> _logger;
    private readonly IAuditService _auditService;
    private readonly ICurrentUserService _currentUserService;

    public UpdateInvoiceCommandHandler(
        AppDbContext context,
        ILogger<UpdateInvoiceCommandHandler> logger,
        IAuditService auditService,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _logger = logger;
        _auditService = auditService;
        _currentUserService = currentUserService;
    }

    public async Task<Result<Domain.Entities.Invoice>> Handle(
    UpdateInvoiceCommand request,
    CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Starting handler {HandlerName}.",
            nameof(UpdateInvoiceCommandHandler));

        try
        {
            if (request.Id != request.Idinvoice)
                throw new ArgumentException("Id mismatch");

            var entity = await _context.Invoices
                .FirstOrDefaultAsync(x => x.Idinvoice == request.Id, cancellationToken);

            if (entity == null)
            {
                return new Result<Domain.Entities.Invoice>(
                    new KeyNotFoundException($"Invoice {request.Id} was not found."));
            }

            if (request.LastModifiedUtc == default)
            {
                return new Result<Domain.Entities.Invoice>(
                    new InvalidOperationException(
                        "This invoice must be reloaded before saving."));
            }

            var dbLastModified = entity.LastModifiedUtc.ToUniversalTime();
            var requestLastModified = request.LastModifiedUtc.ToUniversalTime();

            var diferencia = Math.Abs(
                (dbLastModified - requestLastModified).TotalSeconds);

            if (diferencia > 2)
            {
                return new Result<Domain.Entities.Invoice>(
                    new InvalidOperationException(
                        "This invoice was modified by another user. Please reload it before saving."));
            }

            entity.Invoicenumber = request.Invoicenumber;
            entity.Invoicedate = request.Invoicedate;
            entity.Duedate = request.Duedate;
            entity.Dteordered = request.Dteordered;
            entity.Ponumber = request.Ponumber;
            entity.Memo = request.Memo;
            entity.Subtotal = request.Subtotal;
            entity.Markup = request.Markup;
            entity.Total = request.Total;
            entity.Activeinvoice = request.Activeinvoice;
            entity.Idusers = request.Idusers;
            entity.Idterms = request.Idterms;
            entity.Idrepsales = request.Idrepsales;
            entity.Idcustomermessage = request.Idcustomermessage;
            entity.Customermessagetext = request.Customermessagetext;
            entity.Createdfromestimateid = request.Createdfromestimateid;
            entity.Dateordered = request.Dateordered;
            entity.Openbalance = request.Openbalance;
            entity.Idtaxrate = request.Idtaxrate;
            entity.Taxrate = request.Taxrate;
            entity.Taxamount = request.Taxamount;

            entity.LastModifiedUtc = DateTime.UtcNow;

            _logger.LogInformation(
                "Saving changes for {HandlerName}.",
                nameof(UpdateInvoiceCommandHandler));

            var savedChanges = await _context.SaveChangesAsync(cancellationToken);

            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException(
                    "No changes were saved while handling UpdateInvoiceCommand.");

                _logger.LogWarning(
                    saveException,
                    "No changes were saved for {HandlerName}.",
                    nameof(UpdateInvoiceCommandHandler));

                return new Result<Domain.Entities.Invoice>(saveException);
            }

            _logger.LogInformation(
                "Changes saved for {HandlerName}.",
                nameof(UpdateInvoiceCommandHandler));

            await _auditService.LogAsync(new AuditLogDto
            {
                IdLoginUser = _currentUserService.IdLoginUser,
                Username = _currentUserService.Username,
                Action = AuditActions.InvoiceUpdated,
                EntityName = "Invoices",
                EntityId = entity.Idinvoice.ToString(),
                Description = $"Updated invoice #{entity.Invoicenumber}.",
                PerformedByRole = _currentUserService.UserRole,
                Succeeded = true,
                Severity = AuditSeverity.Information
            });

            return new Result<Domain.Entities.Invoice>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error while handling {HandlerName}.",
                nameof(UpdateInvoiceCommandHandler));

            return new Result<Domain.Entities.Invoice>(ex);
        }
    }

}