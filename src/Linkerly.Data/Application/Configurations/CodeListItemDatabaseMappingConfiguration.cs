using Linkerly.Data.Application.Configurations.Common;
using Linkerly.Domain.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Linkerly.Data.Application.Configurations;

public class CodeListItemDatabaseMappingConfiguration : ChangeTrackingEntityDatabaseMappingConfiguration<CodeListItemEntity>
{
	public override void Configure(EntityTypeBuilder<CodeListItemEntity> builder)
	{
		base.Configure(builder);

		builder.ToTable(Tables.Application.CodeListItems);

		builder.HasKey(codeListItem => codeListItem.CodeListItemID);

		builder.HasIndex(codeListItem => codeListItem.CodeListID)
			.IsUnique(false);

		builder.Property(codeListItem => codeListItem.Value)
			.IsRequired()
			.IsUnicode()
			.HasMaxLength(256);

		builder.HasOne(codeListItem => codeListItem.CodeList)
			.WithMany(codeList => codeList.Items)
			.HasForeignKey(codeListItem => codeListItem.CodeListID)
			.IsRequired()
			.OnDelete(DeleteBehavior.NoAction);
	}
}