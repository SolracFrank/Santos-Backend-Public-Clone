using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Estimates;

public class CreateEstimateCommandHandler
    : IRequestHandler<CreateEstimateCommand, Result<Domain.Entities.Estimate>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateEstimateCommandHandler> _logger;

    public CreateEstimateCommandHandler(AppDbContext context, ILogger<CreateEstimateCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Domain.Entities.Estimate>> Handle(
        CreateEstimateCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            const int maxIntentos = 5;

            for (int intento = 1; intento <= maxIntentos; intento++)
            {
                int estimateNumber = await ObtenerNumeroSeguroAsync(request.Estimatenumber, cancellationToken);

                var entity = new Domain.Entities.Estimate
                {
                    Idestimate = request.Idestimate,
                    Estimatenumber = estimateNumber,
                    Estimatedate = request.Estimatedate,
                    Expdate = request.Expdate,
                    Datetobecompleted = request.Datetobecompleted,
                    Dateordered = request.Dateordered,
                    Memo = request.Memo,
                    Subtotal = request.Subtotal,
                    Markup = request.Markup,
                    Total = request.Total,
                    Activeestimate = request.Activeestimate,
                    Openbalance = request.Openbalance,
                    Idusers = request.Idusers,
                    Idterms = request.Idterms,
                    Idrepsales = request.Idrepsales,
                    Idcustomermessage = request.Idcustomermessage,
                    Customermessagetext = request.Customermessagetext,
                    IsCopy = request.IsCopy,
                    CopiedFromEstimateId = request.CopiedFromEstimateId
                };

                _context.Set<Domain.Entities.Estimate>().Add(entity);

                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    return new Result<Domain.Entities.Estimate>(entity);
                }
                catch (DbUpdateException ex)
                {
                    _context.Entry(entity).State = EntityState.Detached;

                    _logger.LogWarning(ex,
                        "Estimate number conflict. Retrying. Attempt {Attempt}.", intento);

                    if (intento == maxIntentos)
                        return new Result<Domain.Entities.Estimate>(ex);
                }
            }

            return new Result<Domain.Entities.Estimate>(
                new InvalidOperationException("Could not create estimate after several attempts."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(CreateEstimateCommandHandler));
            return new Result<Domain.Entities.Estimate>(ex);
        }
    }

    private async Task<int> ObtenerNumeroSeguroAsync(int numeroSolicitado, CancellationToken cancellationToken)
    {
        if (numeroSolicitado > 0)
        {
            bool exists = await _context.Set<Domain.Entities.Estimate>()
                .AnyAsync(x => x.Estimatenumber == numeroSolicitado, cancellationToken);

            if (!exists)
                return numeroSolicitado;
        }

        int lastNumber = await _context.Set<Domain.Entities.Estimate>()
            .MaxAsync(x => (int?)x.Estimatenumber, cancellationToken) ?? 0;

        return lastNumber + 1;
    }
}