using FluentValidation;

namespace Application.Features.Taxes;

public class CreateTaxCommandValidator : AbstractValidator<CreateTaxCommand>
{
    public CreateTaxCommandValidator()
    {
        RuleFor(x => x.Tname).NotEmpty();
    }
}