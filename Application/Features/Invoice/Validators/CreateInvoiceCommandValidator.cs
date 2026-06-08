using FluentValidation;

namespace Application.Features.Invoices;

public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
{
    public CreateInvoiceCommandValidator()
    {
        RuleFor(x => x).NotNull();
    }
}