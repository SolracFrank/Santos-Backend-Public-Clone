using FluentValidation;

namespace Application.Features.Customers;

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Id).Equal(x => x.Idcustom);
        RuleFor(x => x.Cname).NotEmpty();
    }
}