using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LanguageExt.Common;

namespace Application.Features.Jobinfos;

public class UpdateJobinfoCommandHandler : IRequestHandler<UpdateJobinfoCommand, Result<Jobinfo>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<UpdateJobinfoCommandHandler> _logger;

    public UpdateJobinfoCommandHandler(AppDbContext context, ILogger<UpdateJobinfoCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Jobinfo>> Handle(UpdateJobinfoCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(UpdateJobinfoCommandHandler));

        try
        {
            if (request.Id != request.Idjobinfo)
                throw new ArgumentException("Id mismatch");

            var entity = new Jobinfo
            {
                Idjobinfo = request.Idjobinfo,
                Jdescription = request.Jdescription,
                Jjobstatus = request.Jjobstatus,
                Idjobtype = request.Idjobtype
            };
            _logger.LogInformation("Updating {EntityName} with id {Id}. Handler: {HandlerName}.", "Jobinfo", request.Id, nameof(UpdateJobinfoCommandHandler));
            _context.Entry(entity).State = EntityState.Modified;
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(UpdateJobinfoCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling UpdateJobinfoCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(UpdateJobinfoCommandHandler));
                return new Result<Jobinfo>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(UpdateJobinfoCommandHandler));
            return new Result<Jobinfo>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(UpdateJobinfoCommandHandler));
            return new Result<Jobinfo>(ex);
        }
    }
}