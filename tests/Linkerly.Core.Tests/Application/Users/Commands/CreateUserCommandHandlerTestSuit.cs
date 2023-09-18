using AutoMapper;
using FluentAssertions;
using Linkerly.Core.Application.Users.Commands;
using Linkerly.Core.Application.Users.Mappings;
using Linkerly.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Linkerly.Core.Tests.Application.Users.Commands;

public class CreateUserCommandHandlerTestSuit
{
    [Fact]
    public void Handle_WhenRequestIsValid_ThenInsertNewRecordIntoDatabase()
    {
        // Arrange.
        var mappingProfile = new UserCommandMappingConfiguration();
        var mapperConfiguration = new MapperConfiguration(configuration => configuration.AddProfile(mappingProfile));
        var mapper = new Mapper(mapperConfiguration);

        var databaseOptionsBuilder = new DbContextOptionsBuilder<CloudContext>();

        var connectionString = new SqliteConnectionStringBuilder
        {
            DataSource = ":memory:",
            Mode = SqliteOpenMode.ReadWriteCreate
        }.ToString();

        using var connection = new SqliteConnection(connectionString);

        connection.Open();

        databaseOptionsBuilder.UseSqlite(connection);

        var auditor = new Auditor();
        var databaseOptions = databaseOptionsBuilder.Options;

        using var databaseContext = new CloudContext(databaseOptions, auditor);

        databaseContext.Database.EnsureDeleted();
        databaseContext.Database.EnsureCreated();

        var commandHandler = new CreateUserCommandHandler(databaseContext, mapper);

        var command = new CreateUserCommand("123456789", "givenname.familyname@example.com", true, "givenname \"nickanme\" familyname", "givenname", "familyname", default, "cs");
        var cancellationToken = new CancellationToken();

        // Act.
        commandHandler.Handle(command, cancellationToken);

        // Assert.
        databaseContext.Should().NotBeNull();
        databaseContext.Users.Should().NotBeNull();
        databaseContext.Users.Should().NotBeEmpty();
        databaseContext.Users.Should().HaveCount(1);
    }
}
