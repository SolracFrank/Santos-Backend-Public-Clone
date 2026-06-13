namespace Domain.Models.ConfigurationModels;

using System.IO.Compression;
using System.Text;

public static class EmailAttachmentSecurity
{
    private const int MaxFileNameLength = 150;
    private static readonly UTF8Encoding StrictUtf8 = new(false, true);

    private static readonly IReadOnlyDictionary<string, string[]> ContentTypes =
        new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
        {
            [".pdf"] = ["application/pdf"],
            [".doc"] = ["application/msword"],
            [".docx"] = ["application/vnd.openxmlformats-officedocument.wordprocessingml.document"],
            [".jpg"] = ["image/jpeg"],
            [".jpeg"] = ["image/jpeg"],
            [".webp"] = ["image/webp"],
            [".avif"] = ["image/avif"],
            [".png"] = ["image/png"],
            [".xls"] = ["application/vnd.ms-excel"],
            [".xlsx"] = ["application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"],
            [".txt"] = ["text/plain"]
        };

    public static string NormalizeExtension(string fileName) =>
        Path.GetExtension(Path.GetFileName(fileName)).ToLowerInvariant();

    public static bool IsAllowedExtension(string extension, IEnumerable<string> allowedExtensions)
    {
        string normalizedExtension = NormalizeConfiguredExtension(extension);
        return allowedExtensions.Any(x =>
            string.Equals(NormalizeConfiguredExtension(x), normalizedExtension, StringComparison.OrdinalIgnoreCase));
    }

    public static bool IsSupportedExtension(string extension) =>
        ContentTypes.ContainsKey(NormalizeConfiguredExtension(extension));

    public static bool IsAllowedContentType(string extension, string? contentType)
    {
        if (string.IsNullOrWhiteSpace(contentType) || !ContentTypes.TryGetValue(extension, out string[]? allowed))
            return false;

        string normalizedContentType = contentType.Split(';', 2)[0].Trim();
        return allowed.Contains(normalizedContentType, StringComparer.OrdinalIgnoreCase);
    }

    public static string GetCanonicalContentType(string extension)
    {
        if (!ContentTypes.TryGetValue(extension, out string[]? contentTypes))
            throw new ArgumentException($"La extension '{extension}' no esta soportada.", nameof(extension));

        return contentTypes[0];
    }

    public static string SanitizeFileName(string originalFileName)
    {
        string extension = NormalizeExtension(originalFileName);
        string fileName = Path.GetFileName(originalFileName).Normalize(NormalizationForm.FormKC);
        string stem = Path.GetFileNameWithoutExtension(fileName);

        var sanitizedStem = new StringBuilder(stem.Length);
        foreach (char character in stem)
        {
            if (!char.IsControl(character) && character is not '<' and not '>' and not ':' and not '"'
                and not '/' and not '\\' and not '|' and not '?' and not '*')
            {
                sanitizedStem.Append(character);
            }
        }

        string safeStem = sanitizedStem.ToString().Trim().Trim('.');
        if (string.IsNullOrWhiteSpace(safeStem))
            safeStem = "archivo";

        int maxStemLength = Math.Max(1, MaxFileNameLength - extension.Length);
        if (safeStem.Length > maxStemLength)
            safeStem = safeStem[..maxStemLength].TrimEnd();

        return $"{safeStem}{extension}";
    }

    public static bool HasValidContent(string extension, byte[] content)
    {
        if (content.Length == 0)
            return false;

        return extension.ToLowerInvariant() switch
        {
            ".pdf" => StartsWith(content, "%PDF-"u8),
            ".jpg" or ".jpeg" => content.Length >= 3 && content[0] == 0xFF && content[1] == 0xD8 && content[2] == 0xFF,
            ".png" => StartsWith(content, [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A]),
            ".webp" => content.Length >= 12 && StartsWith(content, "RIFF"u8) && content.AsSpan(8, 4).SequenceEqual("WEBP"u8),
            ".avif" => IsAvif(content),
            ".doc" or ".xls" => StartsWith(content, [0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1]),
            ".docx" => IsOpenXmlDocument(content, "word/"),
            ".xlsx" => IsOpenXmlDocument(content, "xl/"),
            ".txt" => IsPlainText(content),
            _ => false
        };
    }

    private static string NormalizeConfiguredExtension(string extension)
    {
        string trimmed = extension.Trim().ToLowerInvariant();
        return trimmed.StartsWith('.') ? trimmed : $".{trimmed}";
    }

    private static bool StartsWith(byte[] content, ReadOnlySpan<byte> signature) =>
        content.AsSpan().StartsWith(signature);

    private static bool IsAvif(byte[] content)
    {
        if (content.Length < 16 || !content.AsSpan(4, 4).SequenceEqual("ftyp"u8))
            return false;

        for (int offset = 8; offset + 4 <= Math.Min(content.Length, 64); offset += 4)
        {
            ReadOnlySpan<byte> brand = content.AsSpan(offset, 4);
            if (brand.SequenceEqual("avif"u8) || brand.SequenceEqual("avis"u8))
                return true;
        }

        return false;
    }

    private static bool IsOpenXmlDocument(byte[] content, string requiredFolder)
    {
        try
        {
            using var stream = new MemoryStream(content, writable: false);
            using var archive = new ZipArchive(stream, ZipArchiveMode.Read, leaveOpen: false);

            bool hasContentTypes = archive.Entries.Any(entry =>
                string.Equals(entry.FullName, "[Content_Types].xml", StringComparison.OrdinalIgnoreCase));
            bool hasRequiredFolder = archive.Entries.Any(entry =>
                entry.FullName.StartsWith(requiredFolder, StringComparison.OrdinalIgnoreCase));

            return hasContentTypes && hasRequiredFolder;
        }
        catch (InvalidDataException)
        {
            return false;
        }
    }

    private static bool IsPlainText(byte[] content)
    {
        try
        {
            string text = StrictUtf8.GetString(content);
            return text.All(character => character != '\0' && (!char.IsControl(character) || char.IsWhiteSpace(character)));
        }
        catch (DecoderFallbackException)
        {
            return false;
        }
    }
}
