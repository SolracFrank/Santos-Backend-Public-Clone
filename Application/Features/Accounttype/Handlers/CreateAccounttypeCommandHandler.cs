using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using LanguageExt.Common;

namespace Application.Features.Accounttypes;

public class CreateAccounttypeCommandHandler : IRequestHandler<CreateAccounttypeCommand, Result<Accounttype>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateAccounttypeCommandHandler> _logger;

    public CreateAccounttypeCommandHandler(AppDbContext context, ILogger<CreateAccounttypeCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Accounttype>> Handle(CreateAccounttypeCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(CreateAccounttypeCommandHandler));

        try
        {
            var entity = new Accounttype
            {
                Idaccountt = request.Idaccountt,
                Atype = request.Atype
            };
            _logger.LogInformation("Creating {EntityName}. Handler: {HandlerName}.", nameof(Accounttype), nameof(CreateAccounttypeCommandHandler));
            _context.Set<Accounttype>().Add(entity);

            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(CreateAccounttypeCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling CreateAccounttypeCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(CreateAccounttypeCommandHandler));
                return new Result<Accounttype>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(CreateAccounttypeCommandHandler));
            return new Result<Accounttype>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(CreateAccounttypeCommandHandler));

            return new Result<Accounttype>(ex);

        }
    }
}