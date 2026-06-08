using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Users;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<User?>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetUserByIdQueryHandler> _logger;

    public GetUserByIdQueryHandler(AppDbContext context, ILogger<GetUserByIdQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<User?>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetUserByIdQueryHandler));

        try
        {
            _logger.LogInformation("Getting {EntityName} by id {Id}. Handler: {HandlerName}.", "UserById", request.Id, nameof(GetUserByIdQueryHandler));
            var entity = await _context.Users.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity is null)
            {
                var notFoundException = new KeyNotFoundException("User not found");
                _logger.LogWarning(notFoundException, "{EntityName} with id {Id} was not found. Handler: {HandlerName}.", "User", request.Id, nameof(GetUserByIdQueryHandler));
                return new Result<User?>(notFoundException);
            }

            return new Result<User?>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetUserByIdQueryHandler));
            return new Result<User?>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetUserByIdQueryHandler));
        }
    }
}