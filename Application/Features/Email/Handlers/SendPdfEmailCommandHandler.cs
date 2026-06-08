using Domain.Models.ConfigurationModels;
using Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;

namespace Application.Features.Email;

public class SendPdfEmailCommandHandler : IRequestHandler<SendPdfEmailCommand, Result<Unit>>
{
    private const long MaxPdfSizeBytes = 10 * 1024 * 1024;
    private readonly IEmailService _emailService;
    private readonly ILogger<SendPdfEmailCommandHandler> _logger;

    public SendPdfEmailCommandHandler(IEmailService emailService, ILogger<SendPdfEmailCommandHandler> logger)
    {
        _logger = logger;
        _emailService = emailService;
    }

    public async Task<Result<Unit>> Handle(SendPdfEmailCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting handler {HandlerName}.", nameof(SendPdfEmailCommandHandler));

        try
        {
            if (request.Pdf is null)
                throw new ArgumentNullException(nameof(request.Pdf));
        
            if (request.Pdf.Length == 0)
                throw new ArgumentException("El archivo PDF está vacío.");
        
            if (request.Pdf.Length > MaxPdfSizeBytes)
                throw new ArgumentException("El archivo PDF excede el tamaño máximo permitido de 10 MB.");
        
            if (!IsPdf(request.Pdf))
                throw new ArgumentException("El archivo debe ser un PDF válido.");
        
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
        
            _logger.LogInformation(
                "Sending PDF email to {ToEmail} with subject {Subject} and attachment {FileName} ({FileSize} bytes).",
                request.To,
                request.Subject,
                safeFileName,
                request.Pdf.Length);

            await _emailService.SendAsync(
                toEmail: request.To,
                subject: request.Subject,
                htmlBody: htmlBody,
                textBody: request.Message,
                attachments: attachments,
                cancellationToken: cancellationToken);
        
            return new Result<Unit>(Unit.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while handling {HandlerName}.", nameof(SendPdfEmailCommandHandler));
            return new Result<Unit>(ex);
        }
        finally
        {
            _logger.LogInformation("Finished handler {HandlerName}.", nameof(SendPdfEmailCommandHandler));
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