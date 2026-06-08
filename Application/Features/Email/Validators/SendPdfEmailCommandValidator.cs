using FluentValidation;

namespace Application.Features.Email;

public class SendPdfEmailCommandValidator : AbstractValidator<SendPdfEmailCommand>
{
    public SendPdfEmailCommandValidator()
    {
        RuleFor(x => x.To).NotEmpty().WithMessage("El destinatario es requerido.");
        RuleFor(x => x.Subject).NotEmpty().WithMessage("El asunto es requerido.");
        RuleFor(x => x.Message).NotEmpty().WithMessage("El mensaje es requerido.");
        RuleFor(x => x.Pdf).NotNull().WithMessage("El archivo PDF es requerido.");
    }
}