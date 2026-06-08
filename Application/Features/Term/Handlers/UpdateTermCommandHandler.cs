using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LanguageExt.Common;

namespace Application.Features.Terms;

public class UpdateTermCommandHandler : IRequestHandler<UpdateTermCommand, Result<Term>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<UpdateTermCommandHandler> _logger;

    public UpdateTermCommandHandler(AppDbContext context, ILogger<UpdateTermCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Term>> Handle(UpdateTermCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(UpdateTermCommandHandler));

        try
        {
            if (request.Id != request.Idterms)
                throw new ArgumentException("Id mismatch");

            var entity = new Term
            {
                Idterms = request.Idterms,
                Tname = request.Tname,
                Tnets = request.Tnets,
                Tperccs = request.Tperccs,
                Tnetd = request.Tnetd,
                Tdags = request.Tdags,
                Tpercd = request.Tpercd
            };
            _logger.LogInformation("Updating {EntityName} with id {Id}. Handler: {HandlerName}.", "Term", request.Id, nameof(UpdateTermCommandHandler));
            _context.Entry(entity).State = EntityState.Modified;
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(UpdateTermCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling UpdateTermCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(UpdateTermCommandHandler));
                return new Result<Term>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(UpdateTermCommandHandler));
            return new Result<Term>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(UpdateTermCommandHandler));
            return new Result<Term>(ex);
        }
    }
}