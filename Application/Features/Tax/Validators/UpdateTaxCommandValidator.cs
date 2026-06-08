using FluentValidation;

namespace Application.Features.Taxes;

public class UpdateTaxCommandValidator : AbstractValidator<UpdateTaxCommand>
{
    public UpdateTaxCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Id).Equal(x => x.Idtax);
        RuleFor(x => x.Tname).NotEmpty();
    }
}