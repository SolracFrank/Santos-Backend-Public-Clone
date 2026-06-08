using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Addresses;

public class GetAddresssQueryHandler : IRequestHandler<GetAddresssQuery, Result<List<Address>>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetAddresssQueryHandler> _logger;

    public GetAddresssQueryHandler(AppDbContext context, ILogger<GetAddresssQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<List<Address>>> Handle(GetAddresssQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetAddresssQueryHandler));

        try
        {
            _logger.LogInformation("Getting list for {EntityName}. Handler: {HandlerName}.", "Address", nameof(GetAddresssQueryHandler));
            return new Result<List<Address>>(await _context.Addresses.ToListAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetAddresssQueryHandler));
            return new Result<List<Address>>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetAddresssQueryHandler));
        }
    }
}