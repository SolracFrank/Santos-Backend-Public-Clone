using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Terms;

public class GetTermsQueryHandler : IRequestHandler<GetTermsQuery, Result<List<Term>>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetTermsQueryHandler> _logger;

    public GetTermsQueryHandler(AppDbContext context, ILogger<GetTermsQueryHandler> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<List<Term>>> Handle(GetTermsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(GetTermsQueryHandler));

        try
        {
            _logger.LogInformation("Getting list for {EntityName}. Handler: {HandlerName}.", "Term", nameof(GetTermsQueryHandler));
            return new Result<List<Term>>(await _context.Terms.ToListAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(GetTermsQueryHandler));
            return new Result<List<Term>>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(GetTermsQueryHandler));
        }
    }
}