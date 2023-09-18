using Linkerly.Data.Application.Configurations.Common;
using Linkerly.Domain.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Linkerly.Data.Application.Configurations;

public class FolderEntityDatabaseMappingConfiguration : ChangeTrackingEntityDatabaseMappingConfiguration<FolderEntity>
{
    public override void Configure(EntityTypeBuilder<FolderEntity> builder)
    {
        base.Configure(builder);

        builder.ToTable(Tables.Application.Folders);

        builder.HasKey(folder => folder.FolderID);

        builder.HasIndex(folder => folder.UserID)
            .IsUnique(false);

        builder.HasIndex(folder => folder.TypeID)
            .IsUnique(false);

        builder.HasIndex(folder => folder.ParentID)
            .IsUnique(false);

        builder.Property(folder => folder.Name)
            .IsRequired()
            .IsUnicode()
            .HasMaxLength(256);

        builder.Property(folder => folder.TotalSize)
            .IsRequired();

        builder.Property(folder => folder.TotalCount)
            .IsRequired();

        builder.HasOne(folder => folder.Parent)
            .WithMany(folder => folder.Children)
            .HasForeignKey(folder => folder.ParentID)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(folder => folder.Type)
            .WithMany()
            .HasForeignKey(folder => folder.TypeID)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}