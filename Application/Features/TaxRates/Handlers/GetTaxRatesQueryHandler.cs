using Application.Features.TaxRates.Queries;
using Domain.Entities;
using Infrastructure.Data;
using LanguageExt.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Features.TaxRates;

public class GetTaxRatesQueryHandler
    : IRequestHandler<GetTaxRatesQuery, Result<List<TaxRate>>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetTaxRatesQueryHandler> _logger;

    public GetTaxRatesQueryHandler(
        AppDbContext context,
        ILogger<GetTaxRatesQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<List<TaxRate>>> Handle(
        GetTaxRatesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var taxRates = await _context.TaxRates
                .OrderBy(x => x.Name)
                .ToListAsync(cancellationToken);

            return new Result<List<TaxRate>>(taxRates);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error while handling GetTaxRatesQuery");

            return new Result<List<TaxRate>>(ex);
        }
    }
}