using FluentValidation;

namespace Application.Features.Jobtypes;

public class CreateJobtypeCommandValidator : AbstractValidator<CreateJobtypeCommand>
{
    public CreateJobtypeCommandValidator()
    {
        RuleFor(x => x.Jtname).NotEmpty();
    }
}