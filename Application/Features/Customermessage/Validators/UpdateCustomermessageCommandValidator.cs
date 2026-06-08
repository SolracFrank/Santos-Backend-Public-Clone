using FluentValidation;

namespace Application.Features.Customermessages;

public class UpdateCustomermessageCommandValidator : AbstractValidator<UpdateCustomermessageCommand>
{
    public UpdateCustomermessageCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Id).Equal(x => x.Idcustomermessage);
        RuleFor(x => x.Message).NotEmpty();
    }
}