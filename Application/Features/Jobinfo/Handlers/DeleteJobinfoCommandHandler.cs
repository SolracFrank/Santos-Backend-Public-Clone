using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using LanguageExt.Common;

namespace Application.Features.Jobinfos;

public class DeleteJobinfoCommandHandler : IRequestHandler<DeleteJobinfoCommand, Result<Unit>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<DeleteJobinfoCommandHandler> _logger;

    public DeleteJobinfoCommandHandler(AppDbContext context, ILogger<DeleteJobinfoCommandHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Unit>> Handle(DeleteJobinfoCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(DeleteJobinfoCommandHandler));

        try
        {
            var item = await _context.Jobinfos.FindAsync(new object[] { request.Id }, cancellationToken);
            if (item == null)
            {
                _logger.LogWarning("Jobinfo not found. Handler: {HandlerName}.", nameof(DeleteJobinfoCommandHandler));
                throw new KeyNotFoundException("Jobinfo not found");
            }
            _logger.LogInformation("Deleting {EntityName} with id {Id}. Handler: {HandlerName}.", "Jobinfo", request.Id, nameof(DeleteJobinfoCommandHandler));
            _context.Jobinfos.Remove(item);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(DeleteJobinfoCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling DeleteJobinfoCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(DeleteJobinfoCommandHandler));
                return new Result<Unit>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(DeleteJobinfoCommandHandler));
            return new Result<Unit>(Unit.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(DeleteJobinfoCommandHandler));
            return new Result<Unit>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(DeleteJobinfoCommandHandler));
        }
    }
}