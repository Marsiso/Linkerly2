using Linkerly.Domain.Application.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Linkerly.Data;

public class Auditor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is not CloudContext databaseContext)
        {
            return base.SavingChanges(eventData, result);
        }

        OnBeforeSavedChanges(databaseContext);

        return base.SavingChanges(eventData, result);
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        if (eventData.Context is not CloudContext databaseContext)
        {
            return base.SavedChanges(eventData, result);
        }

        OnAfterSavedChanges(databaseContext);

        return base.SavedChanges(eventData, result);
    }

    private static void OnBeforeSavedChanges(CloudContext databaseContext)
    {
        databaseContext.ChangeTracker.DetectChanges();

        DateTime dateTime = DateTime.UtcNow;

        foreach (EntityEntry<ChangeTrackingEntity> entityEntry in databaseContext.ChangeTracker.Entries<ChangeTrackingEntity>())
        {
            switch (entityEntry.State)
            {
                case EntityState.Added:
                {
                    entityEntry.Entity.IsActive = true;
                    entityEntry.Entity.DateCreated = entityEntry.Entity.DateUpdated = dateTime;
                    continue;
                }
                case EntityState.Modified:
                {
                    entityEntry.Entity.DateUpdated = dateTime;
                    continue;
                }
                case EntityState.Deleted:
                {
                    throw new InvalidOperationException();
                }
                case EntityState.Detached:
                {
                    continue;
                }
                case EntityState.Unchanged:
                {
                    continue;
                }
                default:
                {
                    continue;
                }
            }
        }
    }

    private static void OnAfterSavedChanges(CloudContext databaseContext)
    {
    }
}