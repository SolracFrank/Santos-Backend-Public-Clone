using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Customermessages;

public class GetCustomermessageByIdQueryHandler : IRequestHandler<GetCustomermessageByIdQuery, Result<Customermessage?>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetCustomermessageByIdQueryHandler> _logger;

    public GetCustomermessageByIdQueryHandler(AppDbContext context, ILogger<GetCustomermessageByIdQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Customermessage?>> Handle(GetCustomermessageByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetCustomermessageByIdQueryHandler));

        try
        {
            _logger.LogInformation("Getting {EntityName} by id {Id}. Handler: {HandlerName}.", "CustomermessageById", request.Id, nameof(GetCustomermessageByIdQueryHandler));
            var entity = await _context.Customermessages.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity is null)
            {
                var notFoundException = new KeyNotFoundException("Customermessage not found");
                _logger.LogWarning(notFoundException, "{EntityName} with id {Id} was not found. Handler: {HandlerName}.", "Customermessage", request.Id, nameof(GetCustomermessageByIdQueryHandler));
                return new Result<Customermessage?>(notFoundException);
            }

            return new Result<Customermessage?>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetCustomermessageByIdQueryHandler));
            return new Result<Customermessage?>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetCustomermessageByIdQueryHandler));
        }
    }
}