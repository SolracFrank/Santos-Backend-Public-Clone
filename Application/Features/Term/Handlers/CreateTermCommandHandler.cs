using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using LanguageExt.Common;

namespace Application.Features.Terms;

public class CreateTermCommandHandler : IRequestHandler<CreateTermCommand, Result<Term>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateTermCommandHandler> _logger;

    public CreateTermCommandHandler(AppDbContext context, ILogger<CreateTermCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Term>> Handle(CreateTermCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(CreateTermCommandHandler));

        try
        {
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
            _logger.LogInformation("Creating {EntityName}. Handler: {HandlerName}.", nameof(Term), nameof(CreateTermCommandHandler));
            _context.Set<Term>().Add(entity);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(CreateTermCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling CreateTermCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(CreateTermCommandHandler));
                return new Result<Term>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(CreateTermCommandHandler));
            return new Result<Term>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(CreateTermCommandHandler));
            return new Result<Term>(ex);
        }
    }
}