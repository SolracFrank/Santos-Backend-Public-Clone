using FluentValidation;

namespace Application.Features.Invoicedetails;

public class CreateInvoicedetailCommandValidator : AbstractValidator<CreateInvoicedetailCommand>
{
    public CreateInvoicedetailCommandValidator()
    {
        RuleFor(x => x).NotNull();
    }
}