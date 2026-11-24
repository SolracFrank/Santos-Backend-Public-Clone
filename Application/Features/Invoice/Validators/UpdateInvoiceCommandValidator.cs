using FluentValidation;

namespace Application.Features.Invoices;

public class UpdateInvoiceCommandValidator : AbstractValidator<UpdateInvoiceCommand>
{
    public UpdateInvoiceCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Id).Equal(x => x.Idinvoice);
        RuleFor(x => x.Subtotal).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Total).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Openbalance).GreaterThanOrEqualTo(0).LessThanOrEqualTo(x => x.Total);
    }
}
