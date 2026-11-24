using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LanguageExt.Common;
using System.Data;

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

            await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            var previous = await _context.Payments.AsNoTracking()
                .SingleOrDefaultAsync(payment => payment.Idpayment == request.Id, cancellationToken)
                ?? throw new NotFoundException("Payment not found.");
            var amountsApplied = await _context.PaymentApplies
                .Where(allocation => allocation.Idpayment == request.Id)
                .SumAsync(allocation => allocation.AmountApplied, cancellationToken);
            if (amountsApplied > request.Amount || (amountsApplied > 0 && request.Idusers != previous.Idusers))
                throw new ConflictException("An allocated payment must keep its customer and cover the allocated amount.");

            var entity = new Payment
            {
                Idpayment = request.Idpayment,
                Paymentnumber = request.Paymentnumber,
                Paymentdate = request.Paymentdate,
                Amount = request.Amount,
                Memo = request.Memo,
                Idusers = request.Idusers,
                Idpaymd = request.Idpaymd,
                Idaccount = request.Idaccount,
                Reference = request.Reference
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
            await transaction.CommitAsync(cancellationToken);
            return new Result<Payment>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(UpdatePaymentCommandHandler));
            return new Result<Payment>(ex);
        }
    }
}
