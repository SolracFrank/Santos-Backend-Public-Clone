using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Accounttypes;

public class GetAccounttypesQueryHandler : IRequestHandler<GetAccounttypesQuery, Result<List<Accounttype>>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetAccounttypesQueryHandler> _logger;

    public GetAccounttypesQueryHandler(AppDbContext context, ILogger<GetAccounttypesQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<List<Accounttype>>> Handle(GetAccounttypesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetAccounttypesQueryHandler));

        try
        {
            _logger.LogInformation("Getting list for {EntityName}. Handler: {HandlerName}.", "Accounttype", nameof(GetAccounttypesQueryHandler));
            return new Result<List<Accounttype>>(await _context.Accounttypes.ToListAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetAccounttypesQueryHandler));
            return new Result<List<Accounttype>>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetAccounttypesQueryHandler));
        }
    }
}