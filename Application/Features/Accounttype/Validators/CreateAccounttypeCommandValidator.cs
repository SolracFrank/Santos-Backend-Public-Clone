using FluentValidation;

namespace Application.Features.Accounttypes;

public class CreateAccounttypeCommandValidator : AbstractValidator<CreateAccounttypeCommand>
{
    public CreateAccounttypeCommandValidator()
    {
        RuleFor(x => x.Atype).NotEmpty();
    }
}