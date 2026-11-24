using FluentValidation;

namespace Application.Features.PaymentApplies;

public class CreatePaymentApplyCommandValidator : AbstractValidator<CreatePaymentApplyCommand>
{
    public CreatePaymentApplyCommandValidator()
    {
        RuleFor(x => x.Idpayment).GreaterThan(0);
        RuleFor(x => x.Idinvoice).GreaterThan(0);
        RuleFor(x => x.AmountApplied).GreaterThan(0);
    }
}
