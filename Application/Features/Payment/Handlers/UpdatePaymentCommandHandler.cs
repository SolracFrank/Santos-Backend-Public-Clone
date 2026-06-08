using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LanguageExt.Common;

namespace Application.Features.Payments;

public class UpdatePaymentCommandHandler : IRequestHandler<UpdatePaymentCommand, Result<Payment>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<UpdatePaymentCommandHandler> _logger;

    public UpdatePaymentCommandHandler(AppDbContext context, ILogger<UpdatePaymentCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Payment>> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(UpdatePaymentCommandHandler));

        try
        {
            if (request.Id != request.Idpayment)
                throw new ArgumentException("Id mismatch");

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
            _logger.LogInformation("Updating {EntityName} with id {Id}. Handler: {HandlerName}.", "Payment", request.Id, nameof(UpdatePaymentCommandHandler));
            _context.Entry(entity).State = EntityState.Modified;
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(UpdatePaymentCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling UpdatePaymentCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(UpdatePaymentCommandHandler));
                return new Result<Payment>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(UpdatePaymentCommandHandler));
            return new Result<Payment>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(UpdatePaymentCommandHandler));
            return new Result<Payment>(ex);
        }
    }
}