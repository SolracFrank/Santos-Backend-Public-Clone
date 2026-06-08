using FluentValidation;

namespace Application.Features.PaymentApplies;

public class CreatePaymentApplyCommandValidator : AbstractValidator<CreatePaymentApplyCommand>
{
    public CreatePaymentApplyCommandValidator()
    {
        RuleFor(x => x).NotNull();
    }
}