using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Taxes;

public class GetTaxByIdQueryHandler : IRequestHandler<GetTaxByIdQuery, Result<Tax?>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetTaxByIdQueryHandler> _logger;

    public GetTaxByIdQueryHandler(AppDbContext context, ILogger<GetTaxByIdQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Tax?>> Handle(GetTaxByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetTaxByIdQueryHandler));

        try
        {
            _logger.LogInformation("Getting {EntityName} by id {Id}. Handler: {HandlerName}.", "TaxById", request.Id, nameof(GetTaxByIdQueryHandler));
            var entity = await _context.Taxes.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity is null)
            {
                var notFoundException = new KeyNotFoundException("Tax not found");
                _logger.LogWarning(notFoundException, "{EntityName} with id {Id} was not found. Handler: {HandlerName}.", "Tax", request.Id, nameof(GetTaxByIdQueryHandler));
                return new Result<Tax?>(notFoundException);
            }

            return new Result<Tax?>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetTaxByIdQueryHandler));
            return new Result<Tax?>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetTaxByIdQueryHandler));
        }
    }
}