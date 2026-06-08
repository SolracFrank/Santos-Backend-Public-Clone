using FluentValidation;

namespace Application.Features.Estates;

public class CreateEstateCommandValidator : AbstractValidator<CreateEstateCommand>
{
    public CreateEstateCommandValidator()
    {
        RuleFor(x => x.Esname).NotEmpty();
    }
}