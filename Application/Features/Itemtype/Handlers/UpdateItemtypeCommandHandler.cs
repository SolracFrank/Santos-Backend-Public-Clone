using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LanguageExt.Common;

namespace Application.Features.Itemtypes;

public class UpdateItemtypeCommandHandler : IRequestHandler<UpdateItemtypeCommand, Result<Itemtype>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<UpdateItemtypeCommandHandler> _logger;

    public UpdateItemtypeCommandHandler(AppDbContext context, ILogger<UpdateItemtypeCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Itemtype>> Handle(UpdateItemtypeCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(UpdateItemtypeCommandHandler));

        try
        {
            if (request.Id != request.Iditemt)
                throw new ArgumentException("Id mismatch");

            var entity = new Itemtype
            {
                Iditemt = request.Iditemt,
                Itname = request.Itname
            };
            _logger.LogInformation("Updating {EntityName} with id {Id}. Handler: {HandlerName}.", "Itemtype", request.Id, nameof(UpdateItemtypeCommandHandler));
            _context.Entry(entity).State = EntityState.Modified;
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(UpdateItemtypeCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling UpdateItemtypeCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(UpdateItemtypeCommandHandler));
                return new Result<Itemtype>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(UpdateItemtypeCommandHandler));
            return new Result<Itemtype>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(UpdateItemtypeCommandHandler));
            return new Result<Itemtype>(ex);
        }
    }
}