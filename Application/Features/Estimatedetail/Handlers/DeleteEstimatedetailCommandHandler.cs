using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using LanguageExt.Common;

namespace Application.Features.Estimatedetails;

public class DeleteEstimatedetailCommandHandler : IRequestHandler<DeleteEstimatedetailCommand, Result<Unit>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<DeleteEstimatedetailCommandHandler> _logger;

    public DeleteEstimatedetailCommandHandler(AppDbContext context, ILogger<DeleteEstimatedetailCommandHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Unit>> Handle(DeleteEstimatedetailCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(DeleteEstimatedetailCommandHandler));

        try
        {
            var item = await _context.Estimatedetails.FindAsync(new object[] { request.Id }, cancellationToken);
            if (item == null)
            {
                _logger.LogWarning("Estimatedetail not found. Handler: {HandlerName}.", nameof(DeleteEstimatedetailCommandHandler));
                throw new KeyNotFoundException("Estimatedetail not found");
            }
            _logger.LogInformation("Deleting {EntityName} with id {Id}. Handler: {HandlerName}.", "Estimatedetail", request.Id, nameof(DeleteEstimatedetailCommandHandler));
            _context.Estimatedetails.Remove(item);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(DeleteEstimatedetailCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling DeleteEstimatedetailCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(DeleteEstimatedetailCommandHandler));
                return new Result<Unit>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(DeleteEstimatedetailCommandHandler));
            return new Result<Unit>(Unit.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(DeleteEstimatedetailCommandHandler));
            return new Result<Unit>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(DeleteEstimatedetailCommandHandler));
        }
    }
}