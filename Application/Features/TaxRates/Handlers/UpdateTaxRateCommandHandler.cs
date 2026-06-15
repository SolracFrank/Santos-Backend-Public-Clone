using Application.Features.TaxRates.Commands;
using Domain.Entities;
using Infrastructure.Data;
using LanguageExt.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Features.TaxRates;

public class UpdateTaxRateCommandHandler
    : IRequestHandler<UpdateTaxRateCommand, Result<TaxRate>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<UpdateTaxRateCommandHandler> _logger;

    public UpdateTaxRateCommandHandler(
        AppDbContext context,
        ILogger<UpdateTaxRateCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<TaxRate>> Handle(
        UpdateTaxRateCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var taxRate = await _context.TaxRates
                .FirstOrDefaultAsync(x => x.Idtaxrate == request.Idtaxrate, cancellationToken);

            if (taxRate == null)
                return new Result<TaxRate>(new KeyNotFoundException("Tax rate not found."));

            taxRate.Name = request.Name;
            taxRate.Rate = request.Rate;
            taxRate.Active = request.Active;

            await _context.SaveChangesAsync(cancellationToken);

            return new Result<TaxRate>(taxRate);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tax rate {Idtaxrate}", request.Idtaxrate);
            return new Result<TaxRate>(ex);
        }
    }
}