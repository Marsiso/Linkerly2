using Linkerly.Data.Application.Configurations.Common;
using Linkerly.Domain.Application;
using Linkerly.Domain.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Linkerly.Data.Application.Configurations;

public class UserEntityDatabaseMappingConfiguration : ChangeTrackingEntityDatabaseMappingConfiguration<UserEntity>
{
	public override void Configure(EntityTypeBuilder<UserEntity> builder)
	{
		base.Configure(builder);

		builder.ToTable(Tables.Application.Users);

		builder.HasKey(user => user.UserID);

		builder.HasIndex(user => user.Email)
			.IsUnique();

		builder.HasIndex(user => user.IsActive)
			.IsUnique(false);

		builder.Property(user => user.UserID)
			.IsRequired()
			.ValueGeneratedOnAdd();

		builder.Property(user => user.FirstName)
			.IsRequired()
			.IsUnicode()
			.HasMaxLength(256);

		builder.Property(user => user.LastName)
			.IsRequired()
			.IsUnicode()
			.HasMaxLength(256);

		builder.Property(user => user.Email)
			.IsRequired()
			.IsUnicode(false)
			.HasMaxLength(256);

		builder.Property(user => user.ProfilePhotoUrl)
			.IsRequired(false)
			.IsUnicode(false)
			.HasMaxLength(2048);

		builder.HasOne(user => user.RootFolder)
			.WithOne(folder => folder.User)
			.HasForeignKey<FolderEntity>(folder => folder.UserID)
			.IsRequired()
			.OnDelete(DeleteBehavior.NoAction);

		builder.HasOne(user => user.AccessToken)
			.WithOne(token => token.User)
			.HasForeignKey<AccessTokenEntity>(token => token.UserID)
			.IsRequired()
			.OnDelete(DeleteBehavior.NoAction);
	}
}