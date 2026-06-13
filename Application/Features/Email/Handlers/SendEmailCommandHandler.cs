using System.Net;
using Domain.Exceptions;
using Domain.Models.ConfigurationModels;
using Infrastructure.Services.Interfaces;
using LanguageExt.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Features.Email;

public sealed class SendEmailCommandHandler : IRequestHandler<SendEmailCommand, Result<Unit>>
{
    private readonly IEmailService _emailService;
    private readonly ILogger<SendEmailCommandHandler> _logger;
    private readonly EmailAttachmentOptions _attachmentOptions;

    public SendEmailCommandHandler(
        IEmailService emailService,
        ILogger<SendEmailCommandHandler> logger,
        IOptions<EmailAttachmentOptions> attachmentOptions)
    {
        _emailService = emailService;
        _logger = logger;
        _attachmentOptions = attachmentOptions.Value;
    }

    public async Task<Result<Unit>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(SendEmailCommandHandler));

        try
        {
            EmailAttachedConfiguration? mainPdf = request.Pdf is null
                ? null
                : await CreateAttachmentAsync(request.Pdf, cancellationToken);

            var attachments = new List<EmailAttachedConfiguration>(request.Files.Count);

            foreach (var file in request.Files)
            {
                attachments.Add(await CreateAttachmentAsync(file, cancellationToken));
            }

            string htmlBody = $"<p>{WebUtility.HtmlEncode(request.Message).Replace("\r\n", "<br>").Replace("\n", "<br>")}</p>";

            _logger.LogInformation(
                "Sending email to {ToEmail} with subject {Subject}, main PDF: {HasMainPdf}, and {AttachmentCount} additional attachments.",
                request.To,
                request.Subject,
                mainPdf is not null,
                attachments.Count);

            await _emailService.SendAsync(
                toEmail: request.To,
                subject: request.Subject,
                htmlBody: htmlBody,
                textBody: request.Message,
                mainPdf: mainPdf,
                attachments: attachments,
                cancellationToken: cancellationToken);

            return new Result<Unit>(Unit.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(SendEmailCommandHandler));
            return new Result<Unit>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(SendEmailCommandHandler));
        }
    }

    private async Task<EmailAttachedConfiguration> CreateAttachmentAsync(
        Microsoft.AspNetCore.Http.IFormFile file,
        CancellationToken cancellationToken)
    {
        string extension = EmailAttachmentSecurity.NormalizeExtension(file.FileName);
        await using Stream input = file.OpenReadStream();
        using var output = new MemoryStream(capacity: checked((int)file.Length));
        await input.CopyToAsync(output, cancellationToken);
        byte[] content = output.ToArray();

        if (content.LongLength > _attachmentOptions.MaxFileSizeBytes ||
            !EmailAttachmentSecurity.HasValidContent(extension, content))
        {
            throw new BadRequestException($"El archivo '{file.FileName}' no es valido.", "INVALID_EMAIL_ATTACHMENT");
        }

        return new EmailAttachedConfiguration
        {
            FileName = EmailAttachmentSecurity.SanitizeFileName(file.FileName),
            Content = content,
            ContentType = EmailAttachmentSecurity.GetCanonicalContentType(extension)
        };
    }
}
