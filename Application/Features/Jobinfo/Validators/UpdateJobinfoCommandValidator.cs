using FluentValidation;

namespace Application.Features.Jobinfos;

public class UpdateJobinfoCommandValidator : AbstractValidator<UpdateJobinfoCommand>
{
    public UpdateJobinfoCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Id).Equal(x => x.Idjobinfo);
        RuleFor(x => x.Jjobstatus).NotEmpty();
    }
}