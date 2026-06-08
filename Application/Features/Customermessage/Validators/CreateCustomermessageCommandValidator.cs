using FluentValidation;

namespace Application.Features.Customermessages;

public class CreateCustomermessageCommandValidator : AbstractValidator<CreateCustomermessageCommand>
{
    public CreateCustomermessageCommandValidator()
    {
        RuleFor(x => x.Message).NotEmpty();
    }
}