using Linkerly.Domain.Application.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Linkerly.Data;

public class Auditor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is not CloudContext databaseContext) return base.SavingChanges(eventData, result);

        OnBeforeSavedChanges(databaseContext);

        return base.SavingChanges(eventData, result);
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        if (eventData.Context is not CloudContext databaseContext) return base.SavedChanges(eventData, result);

        OnAfterSavedChanges(databaseContext);

        return base.SavedChanges(eventData, result);
    }

    private static void OnBeforeSavedChanges(CloudContext databaseContext)
    {
        databaseContext.ChangeTracker.DetectChanges();

        foreach (var entityEntry in databaseContext.ChangeTracker.Entries<ChangeTrackingEntity>()) UpdateAddedOrModifiedEntry(entityEntry);
    }

    private static void OnAfterSavedChanges(CloudContext databaseContext)
    {
    }

    private static EntityEntry<TEntity> UpdateAddedOrModifiedEntry<TEntity>(EntityEntry<TEntity> entityEntry) where TEntity : ChangeTrackingEntity
    {
        var entity = entityEntry.Entity;

        switch (entityEntry.State)
        {
            case EntityState.Added:
                entity.IsActive = true;
                entity.DateCreated = entity.DateUpdated = DateTime.UtcNow;
                return entityEntry;
            case EntityState.Modified:
                entity.DateUpdated = DateTime.UtcNow;
                return entityEntry;
            case EntityState.Deleted:
                throw new InvalidOperationException();
            case EntityState.Detached:
                return entityEntry;
            case EntityState.Unchanged:
                return entityEntry;
            default:
                return entityEntry;
        }
    }
}
