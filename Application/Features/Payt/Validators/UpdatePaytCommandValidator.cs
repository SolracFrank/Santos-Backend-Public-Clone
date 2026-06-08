using FluentValidation;

namespace Application.Features.Payts;

public class UpdatePaytCommandValidator : AbstractValidator<UpdatePaytCommand>
{
    public UpdatePaytCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Id).Equal(x => x.Idpayt);
    }
}