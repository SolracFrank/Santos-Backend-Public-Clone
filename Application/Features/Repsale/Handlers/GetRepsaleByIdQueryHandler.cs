using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Repsales;

public class GetRepsaleByIdQueryHandler : IRequestHandler<GetRepsaleByIdQuery, Result<Repsale?>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetRepsaleByIdQueryHandler> _logger;

    public GetRepsaleByIdQueryHandler(AppDbContext context, ILogger<GetRepsaleByIdQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Repsale?>> Handle(GetRepsaleByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetRepsaleByIdQueryHandler));

        try
        {
            _logger.LogInformation("Getting {EntityName} by id {Id}. Handler: {HandlerName}.", "RepsaleById", request.Id, nameof(GetRepsaleByIdQueryHandler));
            var entity = await _context.Repsales.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity is null)
            {
                var notFoundException = new KeyNotFoundException("Repsale not found");
                _logger.LogWarning(notFoundException, "{EntityName} with id {Id} was not found. Handler: {HandlerName}.", "Repsale", request.Id, nameof(GetRepsaleByIdQueryHandler));
                return new Result<Repsale?>(notFoundException);
            }

            return new Result<Repsale?>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetRepsaleByIdQueryHandler));
            return new Result<Repsale?>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetRepsaleByIdQueryHandler));
        }
    }
}