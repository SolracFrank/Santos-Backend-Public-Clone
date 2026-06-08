using FluentValidation;

namespace Application.Features.PaymentApplies;

public class UpdatePaymentApplyCommandValidator : AbstractValidator<UpdatePaymentApplyCommand>
{
    public UpdatePaymentApplyCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Id).Equal(x => x.Idpaymentapply);
    }
}