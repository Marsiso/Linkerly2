using Linkerly.Domain.Application.Models.Common;

namespace Linkerly.Domain.Application.Models;

public class FolderEntity : ChangeTrackingEntity
{
	public int FolderID { get; set; }
	public int UserID { get; set; }
	public int? ParentID { get; set; }
	public int TypeID { get; set; }
	public string Name { get; set; } = string.Empty;
	public long TotalSize { get; set; }
	public long TotalCount { get; set; }

	public UserEntity? User { get; set; }
	public CodeListItemEntity? Type { get; set; }
	public FolderEntity? Parent { get; set; }
	public ICollection<FileEntity>? Files { get; set; }
	public ICollection<FolderEntity>? Children { get; set; }
}