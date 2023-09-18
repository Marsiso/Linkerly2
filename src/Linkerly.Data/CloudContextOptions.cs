namespace Linkerly.Data;

public class CloudContextOptions
{
    public const string SectionName = "Sqlite";

    public required string Location { get; set; }
    public required string FileName { get; set; }
}
