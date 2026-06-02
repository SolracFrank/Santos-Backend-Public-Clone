using EsApi.Configuration;
using EsApi.Services.Interfaces;

using MailKit.Net.Smtp;
using MailKit.Security;

using Microsoft.Extensions.Options;

using MimeKit;

namespace EsApi.Services
{
    public class BrevoEmailService : IEmailService
    {
        private readonly BrevoConfiguration _emailSettings;
        private readonly ILogger<BrevoEmailService> _logger;

        public BrevoEmailService(
            IOptions<BrevoConfiguration> emailOptions,
            ILogger<BrevoEmailService> logger)
        {
            _emailSettings = emailOptions.Value;
            _logger = logger;
        }

        public async Task SendAsync(
            string toEmail,
            string subject,
            string htmlBody,
            string? textBody = null,
            IReadOnlyCollection<EmailAttachedConfiguration>? attachments = null,
            CancellationToken cancellationToken = default)
        {
            ValidateEmailSettings();

            if (string.IsNullOrWhiteSpace(toEmail))
                throw new ArgumentException("El destinatario es requerido.", nameof(toEmail));

            if (string.IsNullOrWhiteSpace(subject))
                throw new ArgumentException("El asunto es requerido.", nameof(subject));

            if (string.IsNullOrWhiteSpace(htmlBody))
                throw new ArgumentException("El cuerpo HTML es requerido.", nameof(htmlBody));

            var email = new MimeMessage();

            email.From.Add(new MailboxAddress(
                _emailSettings.FromName,
                _emailSettings.FromEmail));

            email.To.Add(MailboxAddress.Parse(toEmail));

            email.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlBody,
                TextBody = textBody ?? "Tu cliente de correo no soporta HTML."
            };

            if (attachments is not null)
            {
                foreach (EmailAttachedConfiguration attachment in attachments)
                {
                    ValidateAttachment(attachment);

                    bodyBuilder.Attachments.Add(
                        fileName: attachment.FileName,
                        data: attachment.Content,
                        contentType: ContentType.Parse(attachment.ContentType));
                }
            }

            email.Body = bodyBuilder.ToMessageBody();

            using var smtp = new SmtpClient();

            try
            {
                await smtp.ConnectAsync(
                    _emailSettings.SMTP,
                    _emailSettings.Port,
                    SecureSocketOptions.StartTls,
                    cancellationToken);

                await smtp.AuthenticateAsync(
                    _emailSettings.Session,
                    _emailSettings.Password,
                    cancellationToken);

                await smtp.SendAsync(email, cancellationToken);

                await smtp.DisconnectAsync(true, cancellationToken);

                _logger.LogInformation(
                    "Correo enviado a {ToEmail}. Subject: {Subject}. Attachments: {AttachmentCount}",
                    toEmail,
                    subject,
                    attachments?.Count ?? 0);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error enviando correo a {ToEmail}. Subject: {Subject}",
                    toEmail,
                    subject);

                throw;
            }
        }

        private void ValidateEmailSettings()
        {
            if (string.IsNullOrWhiteSpace(_emailSettings.SMTP))
                throw new InvalidOperationException("Email:SMTP no está configurado.");

            if (_emailSettings.Port <= 0)
                throw new InvalidOperationException("Email:Port no es válido.");

            if (string.IsNullOrWhiteSpace(_emailSettings.Session))
                throw new InvalidOperationException("Email:Session no está configurado.");

            if (string.IsNullOrWhiteSpace(_emailSettings.Password))
                throw new InvalidOperationException("Email:Password no está configurado.");

            if (string.IsNullOrWhiteSpace(_emailSettings.FromEmail))
                throw new InvalidOperationException("Email:FromEmail no está configurado.");

            if (string.IsNullOrWhiteSpace(_emailSettings.FromName))
                throw new InvalidOperationException("Email:FromName no está configurado.");
        }

        private static void ValidateAttachment(EmailAttachedConfiguration attachment)
        {
            const int maxAttachmentSizeBytes = 10 * 1024 * 1024;

            if (attachment is null)
                throw new ArgumentNullException(nameof(attachment));

            if (string.IsNullOrWhiteSpace(attachment.FileName))
                throw new InvalidOperationException("El nombre del archivo adjunto es requerido.");

            if (attachment.Content is null || attachment.Content.Length == 0)
                throw new InvalidOperationException($"El archivo adjunto '{attachment.FileName}' está vacío.");

            if (attachment.Content.Length > maxAttachmentSizeBytes)
                throw new InvalidOperationException($"El archivo adjunto '{attachment.FileName}' excede 10 MB.");

            if (string.IsNullOrWhiteSpace(attachment.ContentType))
                throw new InvalidOperationException($"El Content-Type del archivo '{attachment.FileName}' es requerido.");
        }
    }
}
