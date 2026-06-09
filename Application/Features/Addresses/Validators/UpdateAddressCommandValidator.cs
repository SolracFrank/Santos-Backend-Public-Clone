using FluentValidation;

namespace Application.Features.Addresses;

public class UpdateAddressCommandValidator : AbstractValidator<UpdateAddressCommand>
{
    public UpdateAddressCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Id).Equal(x => x.Idaddress);
    }
}