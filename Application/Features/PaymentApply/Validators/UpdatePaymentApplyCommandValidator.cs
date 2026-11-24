using FluentValidation;

namespace Application.Features.PaymentApplies;

public class UpdatePaymentApplyCommandValidator : AbstractValidator<UpdatePaymentApplyCommand>
{
    public UpdatePaymentApplyCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Id).Equal(x => x.Idpaymentapply);
        RuleFor(x => x.Idpayment).GreaterThan(0);
        RuleFor(x => x.Idinvoice).GreaterThan(0);
        RuleFor(x => x.AmountApplied).GreaterThan(0);
    }
}
