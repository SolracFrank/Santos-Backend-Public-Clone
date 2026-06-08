using FluentValidation;

namespace Application.Features.Itservices;

public class CreateItserviceCommandValidator : AbstractValidator<CreateItserviceCommand>
{
    public CreateItserviceCommandValidator()
    {
        RuleFor(x => x.Iname).NotEmpty();
    }
}