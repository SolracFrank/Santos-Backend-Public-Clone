using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using LanguageExt.Common;

namespace Application.Features.TaxRates;

public class CreateTaxRateCommandHandler
    : IRequestHandler<CreateTaxRateCommand, Result<TaxRate>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateTaxRateCommandHandler> _logger;

    public CreateTaxRateCommandHandler(
        AppDbContext context,
        ILogger<CreateTaxRateCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<TaxRate>> Handle(
        CreateTaxRateCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            bool exists = await _context.TaxRates
                .AnyAsync(x => x.Name == request.Name,
                    cancellationToken);

            if (exists)
            {
                return new Result<TaxRate>(
                    new InvalidOperationException(
                        $"Tax Rate '{request.Name}' already exists."));
            }

            var entity = new TaxRate
            {
                Name = request.Name,
                Rate = request.Rate,
                Active = request.Active
            };

            _context.TaxRates.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return new Result<TaxRate>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error while handling CreateTaxRateCommand");

            return new Result<TaxRate>(ex);
        }
    }
}