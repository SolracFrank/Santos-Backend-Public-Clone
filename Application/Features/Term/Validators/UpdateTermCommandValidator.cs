using FluentValidation;

namespace Application.Features.Terms;

public class UpdateTermCommandValidator : AbstractValidator<UpdateTermCommand>
{
    public UpdateTermCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Id).Equal(x => x.Idterms);
        RuleFor(x => x.Tname).NotEmpty();
    }
}