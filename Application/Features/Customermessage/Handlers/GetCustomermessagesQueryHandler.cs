using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Customermessages;

public class GetCustomermessagesQueryHandler : IRequestHandler<GetCustomermessagesQuery, Result<List<Customermessage>>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetCustomermessagesQueryHandler> _logger;

    public GetCustomermessagesQueryHandler(AppDbContext context, ILogger<GetCustomermessagesQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<List<Customermessage>>> Handle(GetCustomermessagesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetCustomermessagesQueryHandler));

        try
        {
            _logger.LogInformation("Getting list for {EntityName}. Handler: {HandlerName}.", "Customermessage", nameof(GetCustomermessagesQueryHandler));
            return new Result<List<Customermessage>>(await _context.Customermessages.ToListAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetCustomermessagesQueryHandler));
            return new Result<List<Customermessage>>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetCustomermessagesQueryHandler));
        }
    }
}