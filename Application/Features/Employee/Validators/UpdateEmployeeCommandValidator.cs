using FluentValidation;

namespace Application.Features.Employees;

public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Id).Equal(x => x.Idemployee);
        RuleFor(x => x.Ename).NotEmpty();
        RuleFor(x => x.Elastname).NotEmpty();
    }
}