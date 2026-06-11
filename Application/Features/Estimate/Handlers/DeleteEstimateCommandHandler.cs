using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using LanguageExt.Common;

namespace Application.Features.Estimates;

public class DeleteEstimateCommandHandler : IRequestHandler<DeleteEstimateCommand, Result<Unit>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<DeleteEstimateCommandHandler> _logger;

    public DeleteEstimateCommandHandler(AppDbContext context, ILogger<DeleteEstimateCommandHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Unit>> Handle(DeleteEstimateCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(DeleteEstimateCommandHandler));

        try
        {
            var item = await _context.Estimates.FindAsync(new object[] { request.Id }, cancellationToken);
            if (item == null)
            {
                _logger.LogWarning("Estimate not found. Handler: {HandlerName}.", nameof(DeleteEstimateCommandHandler));
                throw new KeyNotFoundException("Estimate not found");
            }
            _logger.LogInformation("Hiding {EntityName} with id {Id}. Handler: {HandlerName}.", "Estimate", request.Id, nameof(DeleteEstimateCommandHandler));

            item.Activeestimate = false;

            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(DeleteEstimateCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling DeleteEstimateCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(DeleteEstimateCommandHandler));
                return new Result<Unit>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(DeleteEstimateCommandHandler));
            return new Result<Unit>(Unit.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(DeleteEstimateCommandHandler));
            return new Result<Unit>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(DeleteEstimateCommandHandler));
        }
    }
}