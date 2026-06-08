namespace Domain.Models.ConfigurationModels;

public class EmailAttachedConfiguration
{
    public required string FileName { get; init; }

    public required byte[] Content { get; init; }

    public required string ContentType { get; init; }
}
