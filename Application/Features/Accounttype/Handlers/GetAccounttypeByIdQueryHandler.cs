using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Accounttypes;

public class GetAccounttypeByIdQueryHandler : IRequestHandler<GetAccounttypeByIdQuery, Result<Accounttype?>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetAccounttypeByIdQueryHandler> _logger;

    public GetAccounttypeByIdQueryHandler(AppDbContext context, ILogger<GetAccounttypeByIdQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Accounttype?>> Handle(GetAccounttypeByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetAccounttypeByIdQueryHandler));

        try
        {
            _logger.LogInformation("Getting {EntityName} by id {Id}. Handler: {HandlerName}.", "AccounttypeById", request.Id, nameof(GetAccounttypeByIdQueryHandler));
            var entity = await _context.Accounttypes.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity is null)
            {
                var notFoundException = new KeyNotFoundException("Accounttype not found");
                _logger.LogWarning(notFoundException, "{EntityName} with id {Id} was not found. Handler: {HandlerName}.", "Accounttype", request.Id, nameof(GetAccounttypeByIdQueryHandler));
                return new Result<Accounttype?>(notFoundException);
            }

            return new Result<Accounttype?>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetAccounttypeByIdQueryHandler));
            return new Result<Accounttype?>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetAccounttypeByIdQueryHandler));
        }
    }
}