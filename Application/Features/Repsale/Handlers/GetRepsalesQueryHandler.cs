using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Repsales;

public class GetRepsalesQueryHandler : IRequestHandler<GetRepsalesQuery, Result<List<Repsale>>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetRepsalesQueryHandler> _logger;

    public GetRepsalesQueryHandler(AppDbContext context, ILogger<GetRepsalesQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<List<Repsale>>> Handle(GetRepsalesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetRepsalesQueryHandler));

        try
        {
            _logger.LogInformation("Getting list for {EntityName}. Handler: {HandlerName}.", "Repsale", nameof(GetRepsalesQueryHandler));
            return new Result<List<Repsale>>(await _context.Repsales.ToListAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetRepsalesQueryHandler));
            return new Result<List<Repsale>>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetRepsalesQueryHandler));
        }
    }
}