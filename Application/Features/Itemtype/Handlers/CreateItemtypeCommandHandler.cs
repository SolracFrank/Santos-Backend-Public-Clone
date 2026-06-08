using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using LanguageExt.Common;

namespace Application.Features.Itemtypes;

public class CreateItemtypeCommandHandler : IRequestHandler<CreateItemtypeCommand, Result<Itemtype>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateItemtypeCommandHandler> _logger;

    public CreateItemtypeCommandHandler(AppDbContext context, ILogger<CreateItemtypeCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Itemtype>> Handle(CreateItemtypeCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(CreateItemtypeCommandHandler));

        try
        {
            var entity = new Itemtype
            {
                Iditemt = request.Iditemt,
                Itname = request.Itname
            };
            _logger.LogInformation("Creating {EntityName}. Handler: {HandlerName}.", nameof(Itemtype), nameof(CreateItemtypeCommandHandler));
            _context.Set<Itemtype>().Add(entity);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(CreateItemtypeCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling CreateItemtypeCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(CreateItemtypeCommandHandler));
                return new Result<Itemtype>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(CreateItemtypeCommandHandler));
            return new Result<Itemtype>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(CreateItemtypeCommandHandler));
            return new Result<Itemtype>(ex);
        }
    }
}