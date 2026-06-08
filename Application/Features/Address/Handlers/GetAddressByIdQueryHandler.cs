using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Addresses;

public class GetAddressByIdQueryHandler : IRequestHandler<GetAddressByIdQuery, Result<Address?>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetAddressByIdQueryHandler> _logger;

    public GetAddressByIdQueryHandler(AppDbContext context, ILogger<GetAddressByIdQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Address?>> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetAddressByIdQueryHandler));

        try
        {
            _logger.LogInformation("Getting {EntityName} by id {Id}. Handler: {HandlerName}.", "AddressById", request.Id, nameof(GetAddressByIdQueryHandler));
            var entity = await _context.Addresses.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity is null)
            {
                var notFoundException = new KeyNotFoundException("Address not found");
                _logger.LogWarning(notFoundException, "{EntityName} with id {Id} was not found. Handler: {HandlerName}.", "Address", request.Id, nameof(GetAddressByIdQueryHandler));
                return new Result<Address?>(notFoundException);
            }

            return new Result<Address?>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetAddressByIdQueryHandler));
            return new Result<Address?>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetAddressByIdQueryHandler));
        }
    }
}