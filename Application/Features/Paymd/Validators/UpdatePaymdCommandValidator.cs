using FluentValidation;

namespace Application.Features.Paymds;

public class UpdatePaymdCommandValidator : AbstractValidator<UpdatePaymdCommand>
{
    public UpdatePaymdCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Id).Equal(x => x.Idpaymd);
        RuleFor(x => x.Pmdname).NotEmpty();
    }
}