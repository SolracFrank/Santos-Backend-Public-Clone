using FluentValidation;

namespace Application.Features.Repsales;

public class CreateRepsaleCommandValidator : AbstractValidator<CreateRepsaleCommand>
{
    public CreateRepsaleCommandValidator()
    {
        RuleFor(x => x.Rinitials).NotEmpty();
    }
}