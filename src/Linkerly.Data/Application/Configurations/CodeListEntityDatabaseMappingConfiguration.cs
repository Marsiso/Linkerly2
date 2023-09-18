using Linkerly.Data.Application.Configurations.Common;
using Linkerly.Domain.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Linkerly.Data.Application.Configurations;

public class CodeListEntityDatabaseMappingConfiguration : ChangeTrackingEntityDatabaseMappingConfiguration<CodeListEntity>
{
    public override void Configure(EntityTypeBuilder<CodeListEntity> builder)
    {
        base.Configure(builder);

        builder.ToTable(Tables.Application.CodeLists);

        builder.HasKey(codeList => codeList.CodeListID);

        builder.Property(codeList => codeList.Name)
            .IsRequired()
            .IsUnicode()
            .HasMaxLength(256);
    }
}