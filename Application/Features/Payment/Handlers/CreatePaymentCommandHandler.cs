using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using Domain.Common.Audit;
using Domain.Common.Security;
using Domain.Entities.Dtos;

namespace Application.Features.Payments;

public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Result<Payment>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreatePaymentCommandHandler> _logger;
    private readonly IAuditService _auditService;
    private readonly ICurrentUserService _currentUserService;

    public CreatePaymentCommandHandler(
        AppDbContext context,
        ILogger<CreatePaymentCommandHandler> logger,
        IAuditService auditService,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _logger = logger;
        _auditService = auditService;
        _currentUserService = currentUserService;
    }

    public async Task<Result<Payment>> Handle(
    CreatePaymentCommand request,
    CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Starting handler {HandlerName}.",
            nameof(CreatePaymentCommandHandler));

        try
        {
            const int maxIntentos = 5;

            for (int intento = 1; intento <= maxIntentos; intento++)
            {
                int paymentNumber = await ObtenerNumeroSeguroAsync(
                    request.Paymentnumber,
                    cancellationToken);

                var entity = new Payment
                {
                    Idpayment = request.Idpayment,
                    Paymentnumber = paymentNumber,
                    Paymentdate = request.Paymentdate,
                    Amount = request.Amount,
                    Memo = request.Memo,
                    Idusers = request.Idusers,
                    Idpaymd = request.Idpaymd,
                    Idaccount = request.Idaccount
                };

                _logger.LogInformation(
                    "Creating {EntityName}. Handler: {HandlerName}.",
                    nameof(Payment),
                    nameof(CreatePaymentCommandHandler));

                _context.Set<Payment>().Add(entity);

                try
                {
                    await _context.SaveChangesAsync(cancellationToken);

                    _logger.LogInformation(
                        "Payment #{PaymentNumber} created successfully.",
                        entity.Paymentnumber);

                    await _auditService.LogAsync(new AuditLogDto
                    {
                        IdLoginUser = _currentUserService.IdLoginUser,
                        Username = _currentUserService.Username,
                        Action = AuditActions.PaymentCreated,
                        EntityName = "Payments",
                        EntityId = entity.Idpayment.ToString(),
                        Description = $"Created payment #{entity.Paymentnumber} for {entity.Amount:C2}.",
                        PerformedByRole = _currentUserService.UserRole,
                        Succeeded = true,
                        Severity = AuditSeverity.Information
                    });

                    return new Result<Payment>(entity);
                }
                catch (DbUpdateException ex)
                {
                    _context.Entry(entity).State = EntityState.Detached;

                    _logger.LogWarning(
                        ex,
                        "Payment number conflict. Retrying. Attempt {Attempt}.",
                        intento);

                    if (intento == maxIntentos)
                        return new Result<Payment>(ex);
                }
            }

            return new Result<Payment>(
                new InvalidOperationException(
                    "Could not create payment after several attempts."));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error while handling {HandlerName}.",
                nameof(CreatePaymentCommandHandler));

            return new Result<Payment>(ex);
        }


    }
    private async Task<int> ObtenerNumeroSeguroAsync(
    int numeroSolicitado,
    CancellationToken cancellationToken)
    {
        if (numeroSolicitado > 0)
        {
            bool exists = await _context.Set<Payment>()
                .AnyAsync(x => x.Paymentnumber == numeroSolicitado, cancellationToken);

            if (!exists)
                return numeroSolicitado;
        }

        int lastNumber = await _context.Set<Payment>()
            .MaxAsync(x => (int?)x.Paymentnumber, cancellationToken) ?? 0;

        return lastNumber + 1;
    }
}