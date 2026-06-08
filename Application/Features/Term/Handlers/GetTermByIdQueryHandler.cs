using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Terms;

public class GetTermByIdQueryHandler : IRequestHandler<GetTermByIdQuery, Result<Term?>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetTermByIdQueryHandler> _logger;

    public GetTermByIdQueryHandler(AppDbContext context, ILogger<GetTermByIdQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Term?>> Handle(GetTermByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetTermByIdQueryHandler));

        try
        {
            _logger.LogInformation("Getting {EntityName} by id {Id}. Handler: {HandlerName}.", "TermById", request.Id, nameof(GetTermByIdQueryHandler));
            var entity = await _context.Terms.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity is null)
            {
                var notFoundException = new KeyNotFoundException("Term not found");
                _logger.LogWarning(notFoundException, "{EntityName} with id {Id} was not found. Handler: {HandlerName}.", "Term", request.Id, nameof(GetTermByIdQueryHandler));
                return new Result<Term?>(notFoundException);
            }

            return new Result<Term?>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetTermByIdQueryHandler));
            return new Result<Term?>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetTermByIdQueryHandler));
        }
    }
}