using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Othernames;

public class GetOthernameByIdQueryHandler : IRequestHandler<GetOthernameByIdQuery, Result<Othername?>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetOthernameByIdQueryHandler> _logger;

    public GetOthernameByIdQueryHandler(AppDbContext context, ILogger<GetOthernameByIdQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Othername?>> Handle(GetOthernameByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetOthernameByIdQueryHandler));

        try
        {
            _logger.LogInformation("Getting {EntityName} by id {Id}. Handler: {HandlerName}.", "OthernameById", request.Id, nameof(GetOthernameByIdQueryHandler));
            var entity = await _context.Othernames.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity is null)
            {
                var notFoundException = new KeyNotFoundException("Othername not found");
                _logger.LogWarning(notFoundException, "{EntityName} with id {Id} was not found. Handler: {HandlerName}.", "Othername", request.Id, nameof(GetOthernameByIdQueryHandler));
                return new Result<Othername?>(notFoundException);
            }

            return new Result<Othername?>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetOthernameByIdQueryHandler));
            return new Result<Othername?>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetOthernameByIdQueryHandler));
        }
    }
}