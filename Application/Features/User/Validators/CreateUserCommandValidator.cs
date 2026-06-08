using FluentValidation;

namespace Application.Features.Users;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Ucustomer).NotEmpty();
    }
}