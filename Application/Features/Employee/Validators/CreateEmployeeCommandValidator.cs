using FluentValidation;

namespace Application.Features.Employees;

public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        RuleFor(x => x.Ename).NotEmpty();
        RuleFor(x => x.Elastname).NotEmpty();
    }
}