using FluentValidation;

namespace Application.Features.Accounts;

public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
{
    public UpdateAccountCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Id).Equal(x => x.Idaccount);
        RuleFor(x => x.Aname).NotEmpty();
    }
}