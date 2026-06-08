using FluentValidation;

namespace Application.Features.Paymds;

public class CreatePaymdCommandValidator : AbstractValidator<CreatePaymdCommand>
{
    public CreatePaymdCommandValidator()
    {
        RuleFor(x => x.Pmdname).NotEmpty();
    }
}