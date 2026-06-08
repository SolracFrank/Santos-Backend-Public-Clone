
using Domain.Models.ConfigurationModels;

namespace Infrastructure.Services.Interfaces;

public interface IEmailService
{
    Task SendAsync(
        string toEmail,
        string subject,
        string htmlBody,
        string? textBody = null,
        IReadOnlyCollection<EmailAttachedConfiguration>? attachments = null,
        CancellationToken cancellationToken = default);
}
