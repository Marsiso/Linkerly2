using FluentAssertions;
using Linkerly.Core.Application.Users.Queries;
using Linkerly.Core.Tests.Helpers;
using Linkerly.Domain.Application.Models;

namespace Linkerly.Core.Tests.Application.Users.Queries;

public class GetUserQueryHandlerTestSuit
{
    [Fact]
    public void Handle_WhenEntityExists_ThenReturnValue()
    {
        // Arrange.
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

        _ = databaseContext.Users.Add(userSample);
        _ = databaseContext.SaveChanges();

        var queryHandler = new GetUserQueryHandler(databaseContext);
        var query = new GetUserQuery(userSample.UserID);
        var cancellationToken = new CancellationToken();

        // Act.
        var originalUser = queryHandler.Handle(query, cancellationToken).GetAwaiter().GetResult();

        // Assert.
        originalUser.Should().NotBeNull();
    }

    [Fact]
    public void Handle_WhenEntityExistsNot_ThenReturnDefaultValue()
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

        var queryHandler = new GetUserQueryHandler(databaseContext);
        var query = new GetUserQuery(userSample.UserID);
        var cancellationToken = new CancellationToken();

        // Act.
        var originalUser = queryHandler.Handle(query, cancellationToken).GetAwaiter().GetResult();

        // Assert.
        originalUser.Should().BeNull();
    }
}
