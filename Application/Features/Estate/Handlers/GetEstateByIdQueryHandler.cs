using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Estates;

public class GetEstateByIdQueryHandler : IRequestHandler<GetEstateByIdQuery, Result<Estate?>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetEstateByIdQueryHandler> _logger;

    public GetEstateByIdQueryHandler(AppDbContext context, ILogger<GetEstateByIdQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Estate?>> Handle(GetEstateByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetEstateByIdQueryHandler));

        try
        {
            _logger.LogInformation("Getting {EntityName} by id {Id}. Handler: {HandlerName}.", "EstateById", request.Id, nameof(GetEstateByIdQueryHandler));
            var entity = await _context.Estates.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity is null)
            {
                var notFoundException = new KeyNotFoundException("Estate not found");
                _logger.LogWarning(notFoundException, "{EntityName} with id {Id} was not found. Handler: {HandlerName}.", "Estate", request.Id, nameof(GetEstateByIdQueryHandler));
                return new Result<Estate?>(notFoundException);
            }

            return new Result<Estate?>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetEstateByIdQueryHandler));
            return new Result<Estate?>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetEstateByIdQueryHandler));
        }
    }
}