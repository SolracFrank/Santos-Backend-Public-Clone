using Domain.Entities;
using LanguageExt.Common;
using MediatR;

namespace Application.Features.TaxRates.Commands
{
    public record UpdateTaxRateCommand(
    int Id,
    int Idtaxrate,
    string Name,
    decimal Rate,
    bool Active
) : IRequest<Result<TaxRate>>;
}
