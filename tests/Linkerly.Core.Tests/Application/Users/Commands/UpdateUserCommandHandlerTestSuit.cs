using AutoMapper;
using FluentAssertions;
using Linkerly.Core.Application.Users.Commands;
using Linkerly.Core.Application.Users.Mappings;
using Linkerly.Core.Tests.Helpers;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Exceptions;

namespace Linkerly.Core.Tests.Application.Users.Commands;

public class UpdateUserCommandHandlerTestSuit
{
    [Fact]
    public void Handle_WhenRequestIsValid_ThenUpdateRecordInDatabase()
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
            Identifier = "123456789",
            Email = "givenname.familyname@example.com",
            HasEmailConfirmed = true,
            Name = "givenname \"nickanme\" familyname",
            GivenName = "givenname",
            FamilyName = "familyname",
            Picture = default,
            Locale = "cs"
        };

        _ = databaseContext.Users.Add(userSample);
        _ = databaseContext.SaveChanges();

        var commandHandler = new UpdateUserCommandHandler(databaseContext, mapper);

        var command = new UpdateUserCommand(userSample.UserID, "123456789", "other.givenname.familyname@example.com", true, "givenname \"nickanme\" familyname", "givenname", "familyname", default, "cs");
        var cancellationToken = new CancellationToken();

        // Act.
        commandHandler.Handle(command, cancellationToken).GetAwaiter().GetResult();


        // Assert.
        databaseContext.Users.Any(user => user.UserID == command.UserID).Should().BeTrue();
        databaseContext.Users.SingleOrDefault(user => user.UserID == command.UserID)?.Email.Should().BeEquivalentTo(command.Email);
    }

    [Fact]
    public void Handle_WhenEntityNotFound_ThenThrowEntityNotFoundException()
    {
        // Arrange.
        var mappingProfile = new UserCommandMappingConfiguration();
        var mapperConfiguration = new MapperConfiguration(configuration => configuration.AddProfile(mappingProfile));
        var mapper = new Mapper(mapperConfiguration);

        using var databaseContextWrapper = new CloudContextTestWrapper();

        databaseContextWrapper.Migrate();

        var databaseContext = databaseContextWrapper.Context;

        var commandHandler = new UpdateUserCommandHandler(databaseContext, mapper);

        var command = new UpdateUserCommand(1, "123456789", "other.givenname.familyname@example.com", true, "givenname \"nickanme\" familyname", "givenname", "familyname", default, "cs");
        var cancellationToken = new CancellationToken();

        EntityNotFoundException? exception = default;

        // Act.
        try
        {
            commandHandler.Handle(command, cancellationToken).GetAwaiter().GetResult();
        }
        catch (EntityNotFoundException caughtException)
        {
            exception = caughtException;
        }

        // Assert.
        exception.Should().NotBeNull();
        exception?.EntityID.Should().Be("1");
        exception?.EntityTypeName.Should().BeEquivalentTo(nameof(UserEntity));
    }
}
