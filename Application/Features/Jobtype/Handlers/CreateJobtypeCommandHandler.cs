using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using LanguageExt.Common;

namespace Application.Features.Jobtypes;

public class CreateJobtypeCommandHandler : IRequestHandler<CreateJobtypeCommand, Result<Jobtype>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateJobtypeCommandHandler> _logger;

    public CreateJobtypeCommandHandler(AppDbContext context, ILogger<CreateJobtypeCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Jobtype>> Handle(CreateJobtypeCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(CreateJobtypeCommandHandler));

        try
        {
            var entity = new Jobtype
            {
                Idjobtype = request.Idjobtype,
                Jtname = request.Jtname,
                Idjobfk = request.Idjobfk
            };
            _logger.LogInformation("Creating {EntityName}. Handler: {HandlerName}.", nameof(Jobtype), nameof(CreateJobtypeCommandHandler));
            _context.Set<Jobtype>().Add(entity);
            _logger.LogInformation("Saving changes for {HandlerName}.", nameof(CreateJobtypeCommandHandler));
            var savedChanges = await _context.SaveChangesAsync(cancellationToken);
            if (savedChanges <= 0)
            {
                var saveException = new InvalidOperationException($"No changes were saved while handling CreateJobtypeCommand.");
                _logger.LogWarning(saveException, "No changes were saved for {HandlerName}.", nameof(CreateJobtypeCommandHandler));
                return new Result<Jobtype>(saveException);
            }
            _logger.LogInformation("Changes saved for {HandlerName}.", nameof(CreateJobtypeCommandHandler));
            return new Result<Jobtype>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(CreateJobtypeCommandHandler));
            return new Result<Jobtype>(ex);
        }
    }
}