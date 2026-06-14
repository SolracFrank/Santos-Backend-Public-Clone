namespace Domain.Models.ConfigurationModels;

public sealed class EmailAttachmentOptions
{
    public const string SectionName = "EmailAttachments";

    public int MaxFileSizeMb { get; init; } = 12;

    public int MaxFileCount { get; init; } = 10;

    public string[] AllowedExtensions { get; init; } =
    [
        ".pdf",
        ".doc",
        ".docx",
        ".jpg",
        ".jpeg",
        ".webp",
        ".avif",
        ".png",
        ".xls",
        ".xlsx",
        ".txt"
    ];

    public long MaxFileSizeBytes => checked((long)MaxFileSizeMb * 1024 * 1024);
}
