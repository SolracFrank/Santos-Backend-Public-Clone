using FluentValidation;

namespace Application.Features.Jobinfos;

public class CreateJobinfoCommandValidator : AbstractValidator<CreateJobinfoCommand>
{
    public CreateJobinfoCommandValidator()
    {
        RuleFor(x => x.Jjobstatus).NotEmpty();
    }
}