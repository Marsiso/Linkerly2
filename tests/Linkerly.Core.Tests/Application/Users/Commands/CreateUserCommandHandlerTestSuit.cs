using AutoMapper;
using FluentAssertions;
using Linkerly.Core.Application.Users.Commands;
using Linkerly.Core.Application.Users.Mappings;
using Linkerly.Core.Tests.Helpers;
using Linkerly.Domain.Application.Models;

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

        var userSample = new UserEntity
        {
            UserID = 0,
            Identifier = "123456789",
            Email = "givenname.familyname@example.com",
            HasEmailConfirmed = true,
            Name = "givenname \"nickanme\" familyname",
            GivenName = "givenname",
            FamilyName = "familyname",
            Picture = default,
            Locale = "cs"
        };

        var commandHandler = new CreateUserCommandHandler(databaseContext, mapper);
        var command = mapper.Map<CreateUserCommand>(userSample);
        var cancellationToken = new CancellationToken();

        // Act.
        commandHandler.Handle(command, cancellationToken).GetAwaiter().GetResult();

        // Assert.
        databaseContext.Users.Any().Should().BeTrue();
    }
}
