using FluentValidation;

namespace Application.Features.Invoices;

public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
{
    public CreateInvoiceCommandValidator()
    {
        RuleFor(x => x.Subtotal).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Total).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Openbalance).GreaterThanOrEqualTo(0).LessThanOrEqualTo(x => x.Total);
    }
}
