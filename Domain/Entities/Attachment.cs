namespace Domain.Entities;

public class Attachment
{
    public int Idattachment { get; set; }

    public string EntityType { get; set; } = null!;

    public int EntityId { get; set; }

    public string FileName { get; set; } = null!;

    public string FilePath { get; set; } = null!;

    public DateTime UploadDate { get; set; }
}