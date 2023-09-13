using Linkerly.Domain.Application.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Linkerly.Data.Application.Configurations.Common;

public class EntityBaseDatabaseMappingConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : EntityBase
{
	public virtual void Configure(EntityTypeBuilder<TEntity> builder)
	{
		builder.HasIndex(entity => entity.IsActive);

		builder.HasQueryFilter(entity => entity.IsActive);
	}
}