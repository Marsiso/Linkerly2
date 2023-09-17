using Linkerly.Domain.Application.Models.Common;

namespace Linkerly.Domain.Application.Models;

public class UserEntity : ChangeTrackingEntity
{
    public int UserID { get; set; }
    public string Identifier { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool HasEmailConfirmed { get; set; }
    public string Name { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? ProfilePhotoURL { get; set; }

    public FolderEntity? RootFolder { get; set; }
}