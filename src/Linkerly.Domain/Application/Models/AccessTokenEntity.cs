using Linkerly.Domain.Application.Models.Common;

namespace Linkerly.Domain.Application.Models;

public class AccessTokenEntity : ChangeTrackingEntity
{
	public int AccessTokenID { get; set; }
	public int UserID { get; set; }
	public string Issuer { get; set; } = string.Empty;
	public string Subject { get; set; } = string.Empty;
	public string Audience { get; set; } = string.Empty;
	public string IssuedAt { get; set; } = string.Empty;
	public string Expires { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string IsEmailVerified { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;
	public string? Picture { get; set; }
	public string GivenName { get; set; } = string.Empty;
	public string FamilyName { get; set; } = string.Empty;
	public string Locale { get; set; } = string.Empty;

	public UserEntity? User { get; set; }
}