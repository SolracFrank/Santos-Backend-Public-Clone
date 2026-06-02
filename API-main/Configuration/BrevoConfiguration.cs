namespace EsApi.Configuration;

public sealed class BrevoConfiguration
{
    public const string SectionName = "BrevoEmail";

    //Brevo SMTP configuration properties
    public string SMTP { get; init; } = string.Empty;
    public int Port { get; init; } 
    public string Session { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;

    //MailiKt configuration properties

    public string FromEmail { get; init; } = string.Empty;
    public string FromName { get; init; } = string.Empty;
}
