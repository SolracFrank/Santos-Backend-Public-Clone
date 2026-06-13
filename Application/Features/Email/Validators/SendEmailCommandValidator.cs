using Domain.Models.ConfigurationModels;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Application.Features.Email;

public sealed class SendEmailCommandValidator : AbstractValidator<SendEmailCommand>
{
    private readonly EmailAttachmentOptions _options;

    public SendEmailCommandValidator(IOptions<EmailAttachmentOptions> options)
    {
        _options = options.Value;

        RuleFor(x => x.To)
            .NotEmpty().WithMessage("El destinatario es requerido.")
            .EmailAddress().WithMessage("El destinatario no es un correo valido.");

        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage("El asunto es requerido.")
            .MaximumLength(200).WithMessage("El asunto no puede exceder 200 caracteres.")
            .Must(value => value is null || (!value.Contains('\r') && !value.Contains('\n')))
            .WithMessage("El asunto contiene caracteres no permitidos.");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("El mensaje es requerido.");

        RuleFor(x => x.Pdf)
            .CustomAsync((file, context, cancellationToken) =>
                ValidateFileAsync(file, "Pdf", pdfOnly: true, context, cancellationToken))
            .When(x => x.Pdf is not null);

        RuleFor(x => x.Files)
            .Must(files => files.Count <= _options.MaxFileCount)
            .WithMessage($"No se pueden enviar mas de {_options.MaxFileCount} archivos.");

        RuleForEach(x => x.Files)
            .CustomAsync((file, context, cancellationToken) =>
                ValidateFileAsync(file, "Files", pdfOnly: false, context, cancellationToken));
    }

    private async Task ValidateFileAsync(
        IFormFile? file,
        string propertyName,
        bool pdfOnly,
        ValidationContext<SendEmailCommand> context,
        CancellationToken cancellationToken)
    {
        if (file is null)
        {
            context.AddFailure(propertyName, "El archivo no puede ser nulo.");
            return;
        }

        if (file.Length == 0)
        {
            context.AddFailure(propertyName, $"El archivo '{file.FileName}' esta vacio.");
            return;
        }

        if (file.Length > _options.MaxFileSizeBytes)
        {
            context.AddFailure(propertyName, $"El archivo '{file.FileName}' excede el maximo de {_options.MaxFileSizeMb} MB.");
            return;
        }

        string extension = EmailAttachmentSecurity.NormalizeExtension(file.FileName);
        if (pdfOnly && !string.Equals(extension, ".pdf", StringComparison.OrdinalIgnoreCase))
        {
            context.AddFailure(propertyName, "El documento principal debe ser un archivo PDF.");
            return;
        }

        if (!EmailAttachmentSecurity.IsAllowedExtension(extension, _options.AllowedExtensions))
        {
            context.AddFailure(propertyName, $"El tipo de archivo '{extension}' no esta permitido.");
            return;
        }

        if (!EmailAttachmentSecurity.IsAllowedContentType(extension, file.ContentType))
        {
            context.AddFailure(propertyName, $"El Content-Type de '{file.FileName}' no coincide con su extension.");
            return;
        }

        await using Stream input = file.OpenReadStream();
        using var output = new MemoryStream(capacity: checked((int)file.Length));
        await input.CopyToAsync(output, cancellationToken);

        if (output.Length > _options.MaxFileSizeBytes ||
            !EmailAttachmentSecurity.HasValidContent(extension, output.ToArray()))
        {
            context.AddFailure(propertyName, $"El contenido de '{file.FileName}' no corresponde a un archivo {extension} valido.");
        }
    }
}
