using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Jobinfos;

public class GetJobinfoByIdQueryHandler : IRequestHandler<GetJobinfoByIdQuery, Result<Jobinfo?>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetJobinfoByIdQueryHandler> _logger;

    public GetJobinfoByIdQueryHandler(AppDbContext context, ILogger<GetJobinfoByIdQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Jobinfo?>> Handle(GetJobinfoByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetJobinfoByIdQueryHandler));

        try
        {
            _logger.LogInformation("Getting {EntityName} by id {Id}. Handler: {HandlerName}.", "JobinfoById", request.Id, nameof(GetJobinfoByIdQueryHandler));
            var entity = await _context.Jobinfos.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity is null)
            {
                var notFoundException = new KeyNotFoundException("Jobinfo not found");
                _logger.LogWarning(notFoundException, "{EntityName} with id {Id} was not found. Handler: {HandlerName}.", "Jobinfo", request.Id, nameof(GetJobinfoByIdQueryHandler));
                return new Result<Jobinfo?>(notFoundException);
            }

            return new Result<Jobinfo?>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetJobinfoByIdQueryHandler));
            return new Result<Jobinfo?>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetJobinfoByIdQueryHandler));
        }
    }
}