using AutoMapper;
using FluentAssertions;
using Linkerly.Core.Application.Users.Commands;
using Linkerly.Core.Application.Users.Mappings;
using Linkerly.Core.Tests.Helpers;

namespace Linkerly.Core.Tests.Application.Users.Commands;

public class CreateUserCommandHandlerTestSuit
{
    [Fact]
    public void Handle_WhenRequestIsValid_ThenInsertRecordIntoDatabase()
    {
        // Arrange.
        var mappingProfile = new UserCommandMappingConfiguration();
        var mapperConfiguration = new MapperConfiguration(configuration => configuration.AddProfile(mappingProfile));
        var mapper = new Mapper(mapperConfiguration);

        using var databaseContextWrapper = new CloudContextTestWrapper();

        databaseContextWrapper.Migrate();

        var databaseContext = databaseContextWrapper.Context;

        var commandHandler = new CreateUserCommandHandler(databaseContext, mapper);

        var command = new CreateUserCommand("123456789", "givenname.familyname@example.com", true, "givenname \"nickanme\" familyname", "givenname", "familyname", default, "cs");
        var cancellationToken = new CancellationToken();

        // Act.
        commandHandler.Handle(command, cancellationToken).GetAwaiter().GetResult();

        // Assert.
        databaseContext.Users.Any().Should().BeTrue();
    }
}
