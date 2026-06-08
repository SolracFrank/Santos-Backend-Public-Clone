using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Itservices;

public class GetItserviceByIdQueryHandler : IRequestHandler<GetItserviceByIdQuery, Result<Itservice?>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetItserviceByIdQueryHandler> _logger;

    public GetItserviceByIdQueryHandler(AppDbContext context, ILogger<GetItserviceByIdQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Itservice?>> Handle(GetItserviceByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetItserviceByIdQueryHandler));

        try
        {
            _logger.LogInformation("Getting {EntityName} by id {Id}. Handler: {HandlerName}.", "ItserviceById", request.Id, nameof(GetItserviceByIdQueryHandler));
            var entity = await _context.Itservices.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity is null)
            {
                var notFoundException = new KeyNotFoundException("Itservice not found");
                _logger.LogWarning(notFoundException, "{EntityName} with id {Id} was not found. Handler: {HandlerName}.", "Itservice", request.Id, nameof(GetItserviceByIdQueryHandler));
                return new Result<Itservice?>(notFoundException);
            }

            return new Result<Itservice?>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetItserviceByIdQueryHandler));
            return new Result<Itservice?>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetItserviceByIdQueryHandler));
        }
    }
}