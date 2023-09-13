using Linkerly.Domain.Application.Models.Common;

namespace Linkerly.Domain.Application.Models;

public class UserEntity : ChangeTrackingEntity
{
	public int UserID { get; set; }
	public string Email { get; set; } = string.Empty;
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string? ProfilePhotoUrl { get; set; }
	public DateTime? DateLastAccessed { get; set; }

	public AccessTokenEntity? AccessToken { get; set; }
	public FolderEntity? RootFolder { get; set; }
}