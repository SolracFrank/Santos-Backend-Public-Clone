using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Payts;

public class GetPaytByIdQueryHandler : IRequestHandler<GetPaytByIdQuery, Result<Payt?>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetPaytByIdQueryHandler> _logger;

    public GetPaytByIdQueryHandler(AppDbContext context, ILogger<GetPaytByIdQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Payt?>> Handle(GetPaytByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetPaytByIdQueryHandler));

        try
        {
            _logger.LogInformation("Getting {EntityName} by id {Id}. Handler: {HandlerName}.", "PaytById", request.Id, nameof(GetPaytByIdQueryHandler));
            var entity = await _context.Payts.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity is null)
            {
                var notFoundException = new KeyNotFoundException("Payt not found");
                _logger.LogWarning(notFoundException, "{EntityName} with id {Id} was not found. Handler: {HandlerName}.", "Payt", request.Id, nameof(GetPaytByIdQueryHandler));
                return new Result<Payt?>(notFoundException);
            }

            return new Result<Payt?>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetPaytByIdQueryHandler));
            return new Result<Payt?>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetPaytByIdQueryHandler));
        }
    }
}