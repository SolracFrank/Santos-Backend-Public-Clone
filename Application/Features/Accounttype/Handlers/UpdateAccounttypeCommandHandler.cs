using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LanguageExt.Common;

namespace Application.Features.Accounttypes;

public class UpdateAccounttypeCommandHandler : IRequestHandler<UpdateAccounttypeCommand, Result<Accounttype>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<UpdateAccounttypeCommandHandler> _logger;

    public UpdateAccounttypeCommandHandler(AppDbContext context, ILogger<UpdateAccounttypeCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Accounttype>> Handle(UpdateAccounttypeCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(UpdateAccounttypeCommandHandler));

        try
        {
            if (request.Id != request.Idaccountt)
                throw new ArgumentException("Id mismatch");

            var entity = new Accounttype
            {
                Idaccountt = request.Idaccountt,
                Atype = request.Atype
            };
            _logger.LogInformation("Updating {EntityName} with id {Id}. Handler: {HandlerName}.", "Accounttype", request.Id, nameof(UpdateAccounttypeCommandHandler));
            _context.Entry(entity).State = EntityState.Modified;
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(UpdateAccounttypeCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling UpdateAccounttypeCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(UpdateAccounttypeCommandHandler));
                return new Result<Accounttype>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(UpdateAccounttypeCommandHandler));
            return new Result<Accounttype>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(UpdateAccounttypeCommandHandler));
            return new Result<Accounttype>(ex);
        }
    }
}