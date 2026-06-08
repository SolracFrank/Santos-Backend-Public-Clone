using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using LanguageExt.Common;

namespace Application.Features.Paymds;

public class CreatePaymdCommandHandler : IRequestHandler<CreatePaymdCommand, Result<Paymd>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreatePaymdCommandHandler> _logger;

    public CreatePaymdCommandHandler(AppDbContext context, ILogger<CreatePaymdCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Paymd>> Handle(CreatePaymdCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(CreatePaymdCommandHandler));

        try
        {
            var entity = new Paymd
            {
                Idpaymd = request.Idpaymd,
                Pmdname = request.Pmdname,
                Idpayt = request.Idpayt
            };
            _logger.LogInformation("Creating {EntityName}. Handler: {HandlerName}.", nameof(Paymd), nameof(CreatePaymdCommandHandler));
            _context.Set<Paymd>().Add(entity);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(CreatePaymdCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling CreatePaymdCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(CreatePaymdCommandHandler));
                return new Result<Paymd>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(CreatePaymdCommandHandler));
            return new Result<Paymd>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(CreatePaymdCommandHandler));
            return new Result<Paymd>(ex);
        }
    }
}