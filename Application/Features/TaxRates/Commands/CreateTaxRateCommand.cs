using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.TaxRates;

public record CreateTaxRateCommand(
    string Name,
    decimal Rate,
    bool Active
) : IRequest<Result<TaxRate>>;