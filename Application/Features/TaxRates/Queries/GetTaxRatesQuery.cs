using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.TaxRates.Queries;

public record GetTaxRatesQuery()
    : IRequest<Result<List<TaxRate>>>;