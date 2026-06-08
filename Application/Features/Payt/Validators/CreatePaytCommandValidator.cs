using FluentValidation;

namespace Application.Features.Payts;

public class CreatePaytCommandValidator : AbstractValidator<CreatePaytCommand>
{
    public CreatePaytCommandValidator()
    {
        RuleFor(x => x).NotNull();
    }
}