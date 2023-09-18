#nullable disable

using Linkerly.Domain.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Linkerly.Data;

public class CloudContext : DbContext
{
    private readonly ISaveChangesInterceptor _auditor;

    public CloudContext(DbContextOptions<CloudContext> options, ISaveChangesInterceptor auditor) : base(options)
    {
        _auditor = auditor;
    }

    public DbSet<UserEntity> Users { get; set; }
    public DbSet<FolderEntity> Folders { get; set; }
    public DbSet<FileEntity> Files { get; set; }
    public DbSet<CodeListEntity> CodeLists { get; set; }
    public DbSet<CodeListItemEntity> CodeListItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.AddInterceptors(_auditor);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CloudContext).Assembly);
    }
}
