using FluentValidation;

namespace Application.Features.Estates;

public class UpdateEstateCommandValidator : AbstractValidator<UpdateEstateCommand>
{
    public UpdateEstateCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Id).Equal(x => x.Idestate);
        RuleFor(x => x.Esname).NotEmpty();
    }
}