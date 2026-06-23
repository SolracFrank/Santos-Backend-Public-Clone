namespace Infrastructure.Services;

using Azure.Identity;
using Domain.Models.ConfigurationModels;
using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;

public class OutlookGraphEmailService : IEmailService
{
    private readonly MicrosoftGraphEmailConfiguration _emailSettings;
    private readonly EmailAttachmentOptions _attachmentOptions;
    private readonly ILogger<OutlookGraphEmailService> _logger;
    private readonly GraphServiceClient _graphClient;

    public OutlookGraphEmailService(
        IOptions<MicrosoftGraphEmailConfiguration> emailOptions,
        IOptions<EmailAttachmentOptions> attachmentOptions,
        ILogger<OutlookGraphEmailService> logger)
    {
        _emailSettings = emailOptions.Value;
        _attachmentOptions = attachmentOptions.Value;
        _logger = logger;

        ValidateEmailSettings();

        var credential = new ClientSecretCredential(
            _emailSettings.TenantId,
            _emailSettings.ClientId,
            _emailSettings.ClientSecret);

        _graphClient = new GraphServiceClient(
            credential,
            new[] { "https://graph.microsoft.com/.default" });
    }

    public async Task SendAsync(
        string toEmail,
        string subject,
        string htmlBody,
        string? textBody = null,
        EmailAttachedConfiguration? mainPdf = null,
        IReadOnlyCollection<EmailAttachedConfiguration>? attachments = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(toEmail))
            throw new ArgumentException("El destinatario es requerido.", nameof(toEmail));

        if (string.IsNullOrWhiteSpace(subject))
            throw new ArgumentException("El asunto es requerido.", nameof(subject));

        if (string.IsNullOrWhiteSpace(htmlBody))
            throw new ArgumentException("El cuerpo HTML es requerido.", nameof(htmlBody));

        var fromRecipient = new Recipient
        {
            EmailAddress = new EmailAddress
            {
                Address = _emailSettings.FromEmail,
                Name = _emailSettings.FromName
            }
        };

        var message = new Message
        {
            Subject = subject,
            Body = new ItemBody
            {
                ContentType = BodyType.Html,
                Content = htmlBody
            },
            ToRecipients =
    [
        new Recipient
        {
            EmailAddress = new EmailAddress
            {
                Address = toEmail
            }
        }
    ],
            Attachments = []
        };

        if (mainPdf is not null)
        {
            ValidateMainPdf(mainPdf);
            AddAttachment(message, mainPdf);
        }

        if (attachments is not null)
        {
            if (attachments.Count > _attachmentOptions.MaxFileCount)
                throw new InvalidOperationException($"No se pueden enviar mas de {_attachmentOptions.MaxFileCount} archivos adjuntos.");

            foreach (EmailAttachedConfiguration attachment in attachments)
            {
                ValidateAttachment(attachment);
                AddAttachment(message, attachment);
            }
        }

        var requestBody = new Microsoft.Graph.Users.Item.SendMail.SendMailPostRequestBody
        {
            Message = message,
            SaveToSentItems = true
        };

        await _graphClient
            .Users[_emailSettings.FromEmail]
            .SendMail
            .PostAsync(requestBody, cancellationToken: cancellationToken);

        _logger.LogInformation(
            "Correo enviado con Microsoft Graph a {ToEmail}. Subject: {Subject}. Main PDF: {HasMainPdf}. Additional attachments: {AttachmentCount}",
            toEmail,
            subject,
            mainPdf is not null,
            attachments?.Count ?? 0);
    }

    private void ValidateEmailSettings()
    {
        if (string.IsNullOrWhiteSpace(_emailSettings.TenantId))
            throw new InvalidOperationException("MicrosoftGraphEmail:TenantId no está configurado.");

        if (string.IsNullOrWhiteSpace(_emailSettings.ClientId))
            throw new InvalidOperationException("MicrosoftGraphEmail:ClientId no está configurado.");

        if (string.IsNullOrWhiteSpace(_emailSettings.ClientSecret))
            throw new InvalidOperationException("MicrosoftGraphEmail:ClientSecret no está configurado.");

        if (string.IsNullOrWhiteSpace(_emailSettings.FromEmail))
            throw new InvalidOperationException("MicrosoftGraphEmail:FromEmail no está configurado.");

        if (string.IsNullOrWhiteSpace(_emailSettings.FromName))
            throw new InvalidOperationException("MicrosoftGraphEmail:FromName no está configurado.");
    }

    private void ValidateAttachment(EmailAttachedConfiguration attachment)
    {
        if (attachment is null)
            throw new ArgumentNullException(nameof(attachment));

        if (string.IsNullOrWhiteSpace(attachment.FileName))
            throw new InvalidOperationException("El nombre del archivo adjunto es requerido.");

        if (attachment.Content is null || attachment.Content.Length == 0)
            throw new InvalidOperationException($"El archivo adjunto '{attachment.FileName}' está vacío.");

        if (attachment.Content.LongLength > _attachmentOptions.MaxFileSizeBytes)
            throw new InvalidOperationException($"El archivo adjunto '{attachment.FileName}' excede {_attachmentOptions.MaxFileSizeMb} MB.");

        if (string.IsNullOrWhiteSpace(attachment.ContentType))
            throw new InvalidOperationException($"El Content-Type del archivo '{attachment.FileName}' es requerido.");

        string extension = EmailAttachmentSecurity.NormalizeExtension(attachment.FileName);

        if (!EmailAttachmentSecurity.IsAllowedExtension(extension, _attachmentOptions.AllowedExtensions))
            throw new InvalidOperationException($"El tipo de archivo '{extension}' no esta permitido.");

        if (!EmailAttachmentSecurity.IsAllowedContentType(extension, attachment.ContentType))
            throw new InvalidOperationException($"El Content-Type del archivo '{attachment.FileName}' no es valido.");

        if (!EmailAttachmentSecurity.HasValidContent(extension, attachment.Content))
            throw new InvalidOperationException($"El contenido del archivo '{attachment.FileName}' no coincide con su extension.");
    }

    private void ValidateMainPdf(EmailAttachedConfiguration mainPdf)
    {
        ValidateAttachment(mainPdf);

        string extension = EmailAttachmentSecurity.NormalizeExtension(mainPdf.FileName);

        if (!string.Equals(extension, ".pdf", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("El documento principal debe ser un archivo PDF.");
    }

    private static void AddAttachment(Message message, EmailAttachedConfiguration attachment)
    {
        message.Attachments ??= [];

        message.Attachments.Add(new FileAttachment
        {
            OdataType = "#microsoft.graph.fileAttachment",
            Name = EmailAttachmentSecurity.SanitizeFileName(attachment.FileName),
            ContentType = attachment.ContentType,
            ContentBytes = attachment.Content
        });
    }
}