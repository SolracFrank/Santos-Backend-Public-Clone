using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using LanguageExt.Common;

namespace Application.Features.Payts;

public class CreatePaytCommandHandler : IRequestHandler<CreatePaytCommand, Result<Payt>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreatePaytCommandHandler> _logger;

    public CreatePaytCommandHandler(AppDbContext context, ILogger<CreatePaytCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Payt>> Handle(CreatePaytCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(CreatePaytCommandHandler));

        try
        {
            var entity = new Payt
            {
                Idpayt = request.Idpayt,
                Ptname = request.Ptname
            };
            _logger.LogInformation("Creating {EntityName}. Handler: {HandlerName}.", nameof(Payt), nameof(CreatePaytCommandHandler));
            _context.Set<Payt>().Add(entity);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(CreatePaytCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling CreatePaytCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(CreatePaytCommandHandler));
                return new Result<Payt>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(CreatePaytCommandHandler));
            return new Result<Payt>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(CreatePaytCommandHandler));
            return new Result<Payt>(ex);
        }
    }
}