using FluentValidation;

namespace Application.Features.Estimatedetails;

public class UpdateEstimatedetailCommandValidator : AbstractValidator<UpdateEstimatedetailCommand>
{
    public UpdateEstimatedetailCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Id).Equal(x => x.Idestimatedetail);
    }
}