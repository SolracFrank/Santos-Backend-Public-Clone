using Application.Features.TaxRates.Commands;
using FluentValidation;

namespace Application.Features.TaxRates;

public class UpdateTaxRateCommandValidator : AbstractValidator<UpdateTaxRateCommand>
{
    public UpdateTaxRateCommandValidator()
    {
        RuleFor(x => x.Id)
            .Equal(x => x.Idtaxrate)
            .WithMessage("Route id does not match body id.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Rate)
            .GreaterThanOrEqualTo(0);
    }
}