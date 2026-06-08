using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LanguageExt.Common;

namespace Application.Features.Paymds;

public class UpdatePaymdCommandHandler : IRequestHandler<UpdatePaymdCommand, Result<Paymd>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<UpdatePaymdCommandHandler> _logger;

    public UpdatePaymdCommandHandler(AppDbContext context, ILogger<UpdatePaymdCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Paymd>> Handle(UpdatePaymdCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(UpdatePaymdCommandHandler));

        try
        {
            if (request.Id != request.Idpaymd)
                throw new ArgumentException("Id mismatch");

            var entity = new Paymd
            {
                Idpaymd = request.Idpaymd,
                Pmdname = request.Pmdname,
                Idpayt = request.Idpayt
            };
            _logger.LogInformation("Updating {EntityName} with id {Id}. Handler: {HandlerName}.", "Paymd", request.Id, nameof(UpdatePaymdCommandHandler));
            _context.Entry(entity).State = EntityState.Modified;
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(UpdatePaymdCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling UpdatePaymdCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(UpdatePaymdCommandHandler));
                return new Result<Paymd>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(UpdatePaymdCommandHandler));
            return new Result<Paymd>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(UpdatePaymdCommandHandler));
            return new Result<Paymd>(ex);
        }
    }
}