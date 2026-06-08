using FluentValidation;

namespace Application.Features.Estimates;

public class CreateEstimateCommandValidator : AbstractValidator<CreateEstimateCommand>
{
    public CreateEstimateCommandValidator()
    {
        RuleFor(x => x).NotNull();
    }
}