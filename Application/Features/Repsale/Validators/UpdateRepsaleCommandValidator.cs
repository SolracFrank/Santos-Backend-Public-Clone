using FluentValidation;

namespace Application.Features.Repsales;

public class UpdateRepsaleCommandValidator : AbstractValidator<UpdateRepsaleCommand>
{
    public UpdateRepsaleCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Id).Equal(x => x.Idrepsales);
        RuleFor(x => x.Rinitials).NotEmpty();
    }
}