using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using LanguageExt.Common;

namespace Application.Features.Jobinfos;

public class CreateJobinfoCommandHandler : IRequestHandler<CreateJobinfoCommand, Result<Jobinfo>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateJobinfoCommandHandler> _logger;

    public CreateJobinfoCommandHandler(AppDbContext context, ILogger<CreateJobinfoCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Jobinfo>> Handle(CreateJobinfoCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(CreateJobinfoCommandHandler));

        try
        {
            var entity = new Jobinfo
            {
                Idjobinfo = request.Idjobinfo,
                Jdescription = request.Jdescription,
                Jjobstatus = request.Jjobstatus,
                Idjobtype = request.Idjobtype
            };
            _logger.LogInformation("Creating {EntityName}. Handler: {HandlerName}.", nameof(Jobinfo), nameof(CreateJobinfoCommandHandler));
            _context.Set<Jobinfo>().Add(entity);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(CreateJobinfoCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling CreateJobinfoCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(CreateJobinfoCommandHandler));
                return new Result<Jobinfo>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(CreateJobinfoCommandHandler));
            return new Result<Jobinfo>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(CreateJobinfoCommandHandler));
            return new Result<Jobinfo>(ex);
        }
    }
}