using FluentValidation;

namespace Application.Features.Othernames;

public class UpdateOthernameCommandValidator : AbstractValidator<UpdateOthernameCommand>
{
    public UpdateOthernameCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Id).Equal(x => x.Idother);
        RuleFor(x => x.Oname).NotEmpty();
        RuleFor(x => x.Olastname).NotEmpty();
    }
}