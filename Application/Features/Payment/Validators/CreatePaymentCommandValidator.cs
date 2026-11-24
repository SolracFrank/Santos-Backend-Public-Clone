using FluentValidation;

namespace Application.Features.Payments;

public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
        RuleFor(x => x.Amount).GreaterThan(0);
    }
}
