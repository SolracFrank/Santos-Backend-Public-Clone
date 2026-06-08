using FluentValidation;

namespace Application.Features.Estimatedetails;

public class CreateEstimatedetailCommandValidator : AbstractValidator<CreateEstimatedetailCommand>
{
    public CreateEstimatedetailCommandValidator()
    {
        RuleFor(x => x).NotNull();
    }
}