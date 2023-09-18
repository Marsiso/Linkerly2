using Linkerly.Domain.Application.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Linkerly.Data.Application.Configurations.Common;

public class ChangeTrackingEntityDatabaseMappingConfiguration<TChangeTrackingEntity> : EntityBaseDatabaseMappingConfiguration<TChangeTrackingEntity> where TChangeTrackingEntity : ChangeTrackingEntity
{
    public override void Configure(EntityTypeBuilder<TChangeTrackingEntity> builder)
    {
        base.Configure(builder);

        builder.Property(entity => entity.DateCreated)
            .HasDefaultValueSql("datetime('now', 'localtime')")
            .ValueGeneratedOnAdd();

        builder.Property(entity => entity.DateUpdated)
            .HasDefaultValueSql("datetime('now', 'localtime')")
            .ValueGeneratedOnAddOrUpdate();

        builder.HasOne(entity => entity.UserCreatedBy)
            .WithMany()
            .HasForeignKey(entity => entity.CreatedBy)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(entity => entity.UserUpdatedBy)
            .WithMany()
            .HasForeignKey(entity => entity.UpdatedBy)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
