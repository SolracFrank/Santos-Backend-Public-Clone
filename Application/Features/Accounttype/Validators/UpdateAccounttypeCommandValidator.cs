using FluentValidation;

namespace Application.Features.Accounttypes;

public class UpdateAccounttypeCommandValidator : AbstractValidator<UpdateAccounttypeCommand>
{
    public UpdateAccounttypeCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Id).Equal(x => x.Idaccountt);
        RuleFor(x => x.Atype).NotEmpty();
    }
}