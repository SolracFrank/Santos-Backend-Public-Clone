using FluentValidation;

namespace Application.Features.Estimates;

public class UpdateEstimateCommandValidator : AbstractValidator<UpdateEstimateCommand>
{
    public UpdateEstimateCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Id).Equal(x => x.Idestimate);
    }
}