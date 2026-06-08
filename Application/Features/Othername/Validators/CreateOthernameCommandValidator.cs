using FluentValidation;

namespace Application.Features.Othernames;

public class CreateOthernameCommandValidator : AbstractValidator<CreateOthernameCommand>
{
    public CreateOthernameCommandValidator()
    {
        RuleFor(x => x.Oname).NotEmpty();
        RuleFor(x => x.Olastname).NotEmpty();
    }
}