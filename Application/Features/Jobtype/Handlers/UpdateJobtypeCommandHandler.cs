using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LanguageExt.Common;

namespace Application.Features.Jobtypes;

public class UpdateJobtypeCommandHandler : IRequestHandler<UpdateJobtypeCommand, Result<Jobtype>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<UpdateJobtypeCommandHandler> _logger;

    public UpdateJobtypeCommandHandler(AppDbContext context, ILogger<UpdateJobtypeCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Jobtype>> Handle(UpdateJobtypeCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(UpdateJobtypeCommandHandler));

        try
        {
            if (request.Id != request.Idjobtype)
                throw new ArgumentException("Id mismatch");

            var entity = new Jobtype
            {
                Idjobtype = request.Idjobtype,
                Jtname = request.Jtname,
                Idjobfk = request.Idjobfk
            };
            _logger.LogInformation("Updating {EntityName} with id {Id}. Handler: {HandlerName}.", "Jobtype", request.Id, nameof(UpdateJobtypeCommandHandler));
            _context.Entry(entity).State = EntityState.Modified;
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(UpdateJobtypeCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling UpdateJobtypeCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(UpdateJobtypeCommandHandler));
                return new Result<Jobtype>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(UpdateJobtypeCommandHandler));
            return new Result<Jobtype>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(UpdateJobtypeCommandHandler));
            return new Result<Jobtype>(ex);
        }
    }
}