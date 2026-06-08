using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Itemtypes;

public class GetItemtypeByIdQueryHandler : IRequestHandler<GetItemtypeByIdQuery, Result<Itemtype?>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetItemtypeByIdQueryHandler> _logger;

    public GetItemtypeByIdQueryHandler(AppDbContext context, ILogger<GetItemtypeByIdQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Itemtype?>> Handle(GetItemtypeByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetItemtypeByIdQueryHandler));

        try
        {
            _logger.LogInformation("Getting {EntityName} by id {Id}. Handler: {HandlerName}.", "ItemtypeById", request.Id, nameof(GetItemtypeByIdQueryHandler));
            var entity = await _context.Itemtypes.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity is null)
            {
                var notFoundException = new KeyNotFoundException("Itemtype not found");
                _logger.LogWarning(notFoundException, "{EntityName} with id {Id} was not found. Handler: {HandlerName}.", "Itemtype", request.Id, nameof(GetItemtypeByIdQueryHandler));
                return new Result<Itemtype?>(notFoundException);
            }

            return new Result<Itemtype?>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetItemtypeByIdQueryHandler));
            return new Result<Itemtype?>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetItemtypeByIdQueryHandler));
        }
    }
}