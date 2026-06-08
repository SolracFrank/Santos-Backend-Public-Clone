using FluentValidation;

namespace Application.Features.Itemtypes;

public class UpdateItemtypeCommandValidator : AbstractValidator<UpdateItemtypeCommand>
{
    public UpdateItemtypeCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Id).Equal(x => x.Iditemt);
        RuleFor(x => x.Itname).NotEmpty();
    }
}