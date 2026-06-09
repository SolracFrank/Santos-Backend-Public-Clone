using FluentValidation;

namespace Application.Features.Addresses;

public class CreateAddressCommandValidator : AbstractValidator<CreateAddressCommand>
{
    public CreateAddressCommandValidator()
    {
        RuleFor(x => x).NotNull();
    }
}