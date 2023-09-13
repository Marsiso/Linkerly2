using Linkerly.Data.Application.Configurations.Common;
using Linkerly.Domain.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Linkerly.Data.Application.Configurations;

public class FileEntityDatabaseMappingConfiguration : ChangeTrackingEntityDatabaseMappingConfiguration<FileEntity>
{
	public override void Configure(EntityTypeBuilder<FileEntity> builder)
	{
		base.Configure(builder);

		builder.ToTable(Tables.Application.Files);

		builder.HasKey(file => file.FileID);

		builder.HasIndex(file => file.FolderID)
			.IsUnique(false);

		builder.HasIndex(file => file.ExtensionID)
			.IsUnique(false);

		builder.Property(file => file.SafeName)
			.IsRequired()
			.IsUnicode()
			.HasMaxLength(256);

		builder.Property(file => file.UnsafeName)
			.IsRequired()
			.IsUnicode(false)
			.HasMaxLength(256);

		builder.Property(file => file.Location)
			.IsRequired()
			.IsUnicode(false)
			.HasMaxLength(256);

		builder.Property(file => file.Size)
			.IsRequired();

		builder.HasOne(file => file.Folder)
			.WithMany(folder => folder.Files)
			.HasForeignKey(file => file.FolderID)
			.IsRequired()
			.OnDelete(DeleteBehavior.NoAction);

		builder.HasOne(file => file.Extension)
			.WithMany()
			.HasForeignKey(file => file.ExtensionID)
			.IsRequired()
			.OnDelete(DeleteBehavior.NoAction);

		builder.HasOne(file => file.MimeType)
			.WithMany()
			.HasForeignKey(file => file.MimeTypeID)
			.IsRequired()
			.OnDelete(DeleteBehavior.NoAction);
	}
}