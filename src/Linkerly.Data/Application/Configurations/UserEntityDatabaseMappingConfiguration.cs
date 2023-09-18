using Linkerly.Data.Application.Configurations.Common;
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

        builder.HasIndex(user => user.Identifier)
            .IsUnique();

        builder.HasIndex(user => user.Email)
            .IsUnique();

        builder.HasIndex(user => user.IsActive)
            .IsUnique(false);

        builder.Property(user => user.UserID)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(user => user.Identifier)
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(512);

        builder.Property(user => user.Email)
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(256);

        builder.Property(user => user.Name)
            .IsRequired()
            .IsUnicode()
            .HasMaxLength(1024);

        builder.Property(user => user.GivenName)
            .IsRequired()
            .IsUnicode()
            .HasMaxLength(256);

        builder.Property(user => user.FamilyName)
            .IsRequired()
            .IsUnicode()
            .HasMaxLength(256);

        builder.Property(user => user.HasEmailConfirmed)
            .IsRequired();

        builder.Property(user => user.Picture)
            .IsRequired(false)
            .IsUnicode(false)
            .HasMaxLength(2048);

        builder.Property(user => user.Locale)
            .IsRequired(false)
            .IsUnicode(false)
            .HasMaxLength(32);

        builder.HasOne(user => user.RootFolder)
            .WithOne(folder => folder.User)
            .HasForeignKey<FolderEntity>(folder => folder.UserID)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}