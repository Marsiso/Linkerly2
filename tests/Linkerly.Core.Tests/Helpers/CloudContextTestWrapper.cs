using Linkerly.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Linkerly.Core.Tests.Helpers;

public class CloudContextTestWrapper : IDisposable
{
    private bool _disposed;

    public CloudContextTestWrapper()
    {
        var optionsBuilder = new DbContextOptionsBuilder<CloudContext>();

        var connectionString = new SqliteConnectionStringBuilder
        {
            DataSource = ":memory:",
            Mode = SqliteOpenMode.ReadWriteCreate
        }.ToString();

        Connection = new SqliteConnection(connectionString);

        Connection.Open();

        optionsBuilder.UseSqlite(Connection);

        var auditor = new Auditor();
        var databaseOptions = optionsBuilder.Options;

        Context = new CloudContext(databaseOptions, auditor);
    }

    public CloudContext Context { get; }
    public SqliteConnection Connection { get; }

    public void Dispose()
    {
        if (_disposed) return;

        Connection.Dispose();
        Context.Dispose();
        
        GC.SuppressFinalize(this);

        _disposed = true;
    }

    public void Migrate()
    {
        Context.Database.EnsureDeleted();
        Context.Database.EnsureCreated();
    }
}
