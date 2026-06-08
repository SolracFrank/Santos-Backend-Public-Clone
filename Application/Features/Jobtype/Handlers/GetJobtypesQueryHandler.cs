using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Jobtypes;

public class GetJobtypesQueryHandler : IRequestHandler<GetJobtypesQuery, Result<List<Jobtype>>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetJobtypesQueryHandler> _logger;

    public GetJobtypesQueryHandler(AppDbContext context, ILogger<GetJobtypesQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<List<Jobtype>>> Handle(GetJobtypesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetJobtypesQueryHandler));

        try
        {
            _logger.LogInformation("Getting list for {EntityName}. Handler: {HandlerName}.", "Jobtype", nameof(GetJobtypesQueryHandler));
            return new Result<List<Jobtype>>(await _context.Jobtypes.ToListAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetJobtypesQueryHandler));
            return new Result<List<Jobtype>>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetJobtypesQueryHandler));
        }
    }
}