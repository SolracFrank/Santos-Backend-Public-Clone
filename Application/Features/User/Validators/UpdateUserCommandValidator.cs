using FluentValidation;

namespace Application.Features.Users;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Id).Equal(x => x.Idusers);
        RuleFor(x => x.Ucustomer).NotEmpty();
    }
}