using FluentValidation;

namespace Application.Features.Jobtypes;

public class UpdateJobtypeCommandValidator : AbstractValidator<UpdateJobtypeCommand>
{
    public UpdateJobtypeCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Id).Equal(x => x.Idjobtype);
        RuleFor(x => x.Jtname).NotEmpty();
    }
}