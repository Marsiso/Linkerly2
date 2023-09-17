using Linkerly.Domain.Application.Models.Common;

namespace Linkerly.Domain.Application.Models;

public class FileEntity : ChangeTrackingEntity
{
    public int FileID { get; set; }
    public int FolderID { get; set; }
    public int ExtensionID { get; set; }
    public int MimeTypeID { get; set; }
    public string SafeName { get; set; } = string.Empty;
    public string UnsafeName { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public long Size { get; set; }

    public FolderEntity? Folder { get; set; }
    public CodeListItemEntity? Extension { get; set; }
    public CodeListItemEntity? MimeType { get; set; }
}