using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Jobtypes;

public class GetJobtypeByIdQueryHandler : IRequestHandler<GetJobtypeByIdQuery, Result<Jobtype?>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetJobtypeByIdQueryHandler> _logger;

    public GetJobtypeByIdQueryHandler(AppDbContext context, ILogger<GetJobtypeByIdQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Jobtype?>> Handle(GetJobtypeByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetJobtypeByIdQueryHandler));

        try
        {
            _logger.LogInformation("Getting {EntityName} by id {Id}. Handler: {HandlerName}.", "JobtypeById", request.Id, nameof(GetJobtypeByIdQueryHandler));
            var entity = await _context.Jobtypes.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity is null)
            {
                var notFoundException = new KeyNotFoundException("Jobtype not found");
                _logger.LogWarning(notFoundException, "{EntityName} with id {Id} was not found. Handler: {HandlerName}.", "Jobtype", request.Id, nameof(GetJobtypeByIdQueryHandler));
                return new Result<Jobtype?>(notFoundException);
            }

            return new Result<Jobtype?>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetJobtypeByIdQueryHandler));
            return new Result<Jobtype?>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetJobtypeByIdQueryHandler));
        }
    }
}