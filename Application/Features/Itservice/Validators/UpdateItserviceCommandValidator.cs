using FluentValidation;

namespace Application.Features.Itservices;

public class UpdateItserviceCommandValidator : AbstractValidator<UpdateItserviceCommand>
{
    public UpdateItserviceCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Id).Equal(x => x.Iditem);
        RuleFor(x => x.Iname).NotEmpty();
    }
}