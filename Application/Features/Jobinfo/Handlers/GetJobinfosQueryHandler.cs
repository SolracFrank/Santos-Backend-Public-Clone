using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Jobinfos;

public class GetJobinfosQueryHandler : IRequestHandler<GetJobinfosQuery, Result<List<Jobinfo>>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetJobinfosQueryHandler> _logger;

    public GetJobinfosQueryHandler(AppDbContext context, ILogger<GetJobinfosQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<List<Jobinfo>>> Handle(GetJobinfosQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetJobinfosQueryHandler));

        try
        {
            _logger.LogInformation("Getting list for {EntityName}. Handler: {HandlerName}.", "Jobinfo", nameof(GetJobinfosQueryHandler));
            return new Result<List<Jobinfo>>(await _context.Jobinfos.ToListAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetJobinfosQueryHandler));
            return new Result<List<Jobinfo>>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetJobinfosQueryHandler));
        }
    }
}