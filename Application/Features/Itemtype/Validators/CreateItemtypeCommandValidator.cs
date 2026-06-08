using FluentValidation;

namespace Application.Features.Itemtypes;

public class CreateItemtypeCommandValidator : AbstractValidator<CreateItemtypeCommand>
{
    public CreateItemtypeCommandValidator()
    {
        RuleFor(x => x.Itname).NotEmpty();
    }
}