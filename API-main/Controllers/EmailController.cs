namespace EsApi.Controllers;

using EsApi.Configuration;
using EsApi.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

using System.Net;

[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    private const long MaxPdfSizeBytes = 10 * 1024 * 1024; // 10 MB

    private readonly IEmailService _emailService;
    private readonly ILogger<EmailController> _logger;

    public EmailController(
        IEmailService emailService,
        ILogger<EmailController> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    [HttpPost("send-pdf")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> SendPdfEmail(
        [FromForm] SendPdfEmailRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.To))
            return BadRequest("El destinatario es requerido.");

        if (string.IsNullOrWhiteSpace(request.Subject))
            return BadRequest("El asunto es requerido.");

        if (string.IsNullOrWhiteSpace(request.Message))
            return BadRequest("El mensaje es requerido.");

        if (request.Pdf is not null)
        {
            if ( request.Pdf.Length == 0)
                return BadRequest("El archivo PDF está vacío.");

            if (request.Pdf.Length > MaxPdfSizeBytes)
                return BadRequest("El archivo PDF excede el tamaño máximo permitido de 10 MB.");

            if (!IsPdf(request.Pdf))
                return BadRequest("El archivo debe ser un PDF válido.");
        }



        byte[] pdfBytes;

        await using (var memoryStream = new MemoryStream())
        {
            await request.Pdf.CopyToAsync(memoryStream, cancellationToken);
            pdfBytes = memoryStream.ToArray();
        }

        string safeFileName = GetSafePdfFileName(request.Pdf.FileName);

        string htmlBody = $"""
            <p>{WebUtility.HtmlEncode(request.Message).Replace("\n", "<br>")}</p>
            """;

        var attachments = new List<EmailAttachedConfiguration>
        {
            new EmailAttachedConfiguration
            {
                FileName = safeFileName,
                Content = pdfBytes,
                ContentType = "application/pdf"
            }
        };

        try
        {
            await _emailService.SendAsync(
                toEmail: request.To,
                subject: request.Subject,
                htmlBody: htmlBody,
                textBody: request.Message,
                attachments: attachments,
                cancellationToken: cancellationToken);

            return Ok(new
            {
                message = "Correo enviado correctamente."
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error enviando correo con PDF a {ToEmail}",
                request.To);

            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "Ocurrió un error al enviar el correo.");
        }
    }

    private static bool IsPdf(IFormFile file)
    {
        if (!file.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            return false;

        if (!string.Equals(file.ContentType, "application/pdf", StringComparison.OrdinalIgnoreCase))
            return false;

        return true;
    }

    private static string GetSafePdfFileName(string originalFileName)
    {
        string fileName = Path.GetFileName(originalFileName);

        if (string.IsNullOrWhiteSpace(fileName))
            return "documento.pdf";

        if (!fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            return $"{fileName}.pdf";

        return fileName;
    }
}

public sealed class SendPdfEmailRequest
{
    public string To { get; set; } = string.Empty;

    public string Subject { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public IFormFile? Pdf { get; set; }
}