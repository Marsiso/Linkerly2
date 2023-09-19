using FluentAssertions;
using Linkerly.Core.Application.Users.Commands;
using Linkerly.Core.Tests.Helpers;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Linkerly.Core.Tests.Application.Users.Commands;

public class DeleteUserCommandHandlerTestSuit
{
    [Fact]
    public void Handle_WhenRequestIsValid_ThenRemoveRecordFromDatabase()
    {
        // Arrange.
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

        var commandHandler = new DeleteUserCommandHandler(databaseContext);

        var command = new DeleteUserCommand(userSample.UserID);
        var cancellationToken = new CancellationToken();

        // Act.
        commandHandler.Handle(command, cancellationToken).GetAwaiter().GetResult();

        // Assert.
        databaseContext.Users.AsNoTracking().Any(user => user.UserID == command.UserID).Should().BeFalse();
        databaseContext.Users.AsNoTracking().IgnoreQueryFilters().Any(user => user.UserID == command.UserID).Should().BeTrue();
        databaseContext.Users.AsNoTracking().IgnoreQueryFilters().SingleOrDefault(user => user.UserID == command.UserID)?.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Handle_WhenEntityNotFound_ThenThrowEntityNotFoundException()
    {
        // Arrange.
        using var databaseContextWrapper = new CloudContextTestWrapper();

        databaseContextWrapper.Migrate();

        var databaseContext = databaseContextWrapper.Context;

        var userSample = new UserEntity
        {
            UserID = 1,
            Identifier = "123456789",
            Email = "givenname.familyname@example.com",
            HasEmailConfirmed = true,
            Name = "givenname \"nickanme\" familyname",
            GivenName = "givenname",
            FamilyName = "familyname",
            Picture = default,
            Locale = "cs"
        };

        var commandHandler = new DeleteUserCommandHandler(databaseContext);

        var command = new DeleteUserCommand(1);
        var cancellationToken = new CancellationToken();

        // Act.
        Action action = () => commandHandler.Handle(command, cancellationToken).GetAwaiter().GetResult();

        var exception = Record.Exception(action);
        // Assert.
        exception.Should().NotBeNull();
        exception.Should().BeOfType<EntityNotFoundException>();

        (exception as EntityNotFoundException)?.EntityID.Should().BeEquivalentTo(userSample.UserID.ToString());
        (exception as EntityNotFoundException)?.EntityTypeName.Should().BeEquivalentTo(nameof(UserEntity));
    }
}
