using FluentValidation;

namespace Application.Features.Terms;

public class CreateTermCommandValidator : AbstractValidator<CreateTermCommand>
{
    public CreateTermCommandValidator()
    {
        RuleFor(x => x.Tname).NotEmpty();
    }
}