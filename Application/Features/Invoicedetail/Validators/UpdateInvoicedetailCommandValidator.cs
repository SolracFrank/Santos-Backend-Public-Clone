using FluentValidation;

namespace Application.Features.Invoicedetails;

public class UpdateInvoicedetailCommandValidator : AbstractValidator<UpdateInvoicedetailCommand>
{
    public UpdateInvoicedetailCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Id).Equal(x => x.Idinvoicedetail);
    }
}