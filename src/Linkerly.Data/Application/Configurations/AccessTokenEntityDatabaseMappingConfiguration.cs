using Linkerly.Data.Application.Configurations.Common;
using Linkerly.Domain.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Linkerly.Data.Application.Configurations;

public class AccessTokenEntityDatabaseMappingConfiguration : ChangeTrackingEntityDatabaseMappingConfiguration<AccessTokenEntity>
{
	public override void Configure(EntityTypeBuilder<AccessTokenEntity> builder)
	{
		base.Configure(builder);

		builder.ToTable(Tables.Application.AccessTokens);

		builder.HasKey(token => token.AccessTokenID);

		builder.HasIndex(token => token.Subject)
			.IsUnique();

		builder.HasIndex(token => token.UserID)
			.IsUnique(false);

		builder.Property(token => token.Subject)
			.IsRequired()
			.IsUnicode(false)
			.HasMaxLength(512);

		builder.Property(token => token.Issuer)
			.IsRequired()
			.IsUnicode()
			.HasMaxLength(256);

		builder.Property(token => token.Subject)
			.IsRequired()
			.IsUnicode(false)
			.HasMaxLength(512);

		builder.Property(token => token.Email)
			.IsRequired()
			.IsUnicode(false)
			.HasMaxLength(256);

		builder.Property(token => token.GivenName)
			.IsRequired()
			.IsUnicode()
			.HasMaxLength(256);

		builder.Property(token => token.FamilyName)
			.IsRequired()
			.IsUnicode()
			.HasMaxLength(256);

		builder.Property(token => token.Picture)
			.IsRequired(false)
			.IsUnicode(false)
			.HasMaxLength(2048);

		builder.Property(token => token.Locale)
			.IsRequired()
			.IsUnicode()
			.HasMaxLength(32);

		builder.Property(token => token.IsEmailVerified)
			.IsRequired();
	}
}