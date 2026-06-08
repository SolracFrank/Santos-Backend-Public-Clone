using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using LanguageExt.Common;

namespace Application.Features.Payments;

public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Result<Payment>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreatePaymentCommandHandler> _logger;

    public CreatePaymentCommandHandler(AppDbContext context, ILogger<CreatePaymentCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Payment>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(CreatePaymentCommandHandler));

        try
        {
            var entity = new Payment
            {
                Idpayment = request.Idpayment,
                Paymentnumber = request.Paymentnumber,
                Paymentdate = request.Paymentdate,
                Amount = request.Amount,
                Memo = request.Memo,
                Idusers = request.Idusers,
                Idpaymd = request.Idpaymd,
                Idaccount = request.Idaccount
            };
            _logger.LogInformation("Creating {EntityName}. Handler: {HandlerName}.", nameof(Payment), nameof(CreatePaymentCommandHandler));
            _context.Set<Payment>().Add(entity);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(CreatePaymentCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling CreatePaymentCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(CreatePaymentCommandHandler));
                return new Result<Payment>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(CreatePaymentCommandHandler));
            return new Result<Payment>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(CreatePaymentCommandHandler));
            return new Result<Payment>(ex);
        }
    }
}