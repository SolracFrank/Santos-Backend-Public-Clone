using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Paymds;

public class GetPaymdByIdQueryHandler : IRequestHandler<GetPaymdByIdQuery, Result<Paymd?>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetPaymdByIdQueryHandler> _logger;

    public GetPaymdByIdQueryHandler(AppDbContext context, ILogger<GetPaymdByIdQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Paymd?>> Handle(GetPaymdByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetPaymdByIdQueryHandler));

        try
        {
            _logger.LogInformation("Getting {EntityName} by id {Id}. Handler: {HandlerName}.", "PaymdById", request.Id, nameof(GetPaymdByIdQueryHandler));
            var entity = await _context.Paymds.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity is null)
            {
                var notFoundException = new KeyNotFoundException("Paymd not found");
                _logger.LogWarning(notFoundException, "{EntityName} with id {Id} was not found. Handler: {HandlerName}.", "Paymd", request.Id, nameof(GetPaymdByIdQueryHandler));
                return new Result<Paymd?>(notFoundException);
            }

            return new Result<Paymd?>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetPaymdByIdQueryHandler));
            return new Result<Paymd?>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetPaymdByIdQueryHandler));
        }
    }
}