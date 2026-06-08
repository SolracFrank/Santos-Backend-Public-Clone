using FluentValidation;

namespace Application.Features.Accounts;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(x => x.Aname).NotEmpty();
    }
}