using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.PaymentApplies;

public class GetPaymentApplyByIdQueryHandler : IRequestHandler<GetPaymentApplyByIdQuery, Result<PaymentApply?>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetPaymentApplyByIdQueryHandler> _logger;

    public GetPaymentApplyByIdQueryHandler(AppDbContext context, ILogger<GetPaymentApplyByIdQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<PaymentApply?>> Handle(GetPaymentApplyByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetPaymentApplyByIdQueryHandler));

        try
        {
            _logger.LogInformation("Getting {EntityName} by id {Id}. Handler: {HandlerName}.", "PaymentApplyById", request.Id, nameof(GetPaymentApplyByIdQueryHandler));
            var entity = await _context.PaymentApplies.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity is null)
            {
                var notFoundException = new KeyNotFoundException("PaymentApply not found");
                _logger.LogWarning(notFoundException, "{EntityName} with id {Id} was not found. Handler: {HandlerName}.", "PaymentApply", request.Id, nameof(GetPaymentApplyByIdQueryHandler));
                return new Result<PaymentApply?>(notFoundException);
            }

            return new Result<PaymentApply?>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetPaymentApplyByIdQueryHandler));
            return new Result<PaymentApply?>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetPaymentApplyByIdQueryHandler));
        }
    }
}