using FluentAssertions;
using Linkerly.Core.Application.Folders.Queries;
using Linkerly.Core.Tests.Helpers;
using Linkerly.Domain.Application.Models;

namespace Linkerly.Core.Tests.Application.Folders.Queries;

public class GetFolderQueryHandlerTestSuit
{
    [Fact]
    public void Handle_WhenEntityExists_ThenReturnValue()
    {
        // Arrange.

        using var databaseContextWrapper = new CloudContextTestWrapper();

        databaseContextWrapper.Migrate();

        var databaseContext = databaseContextWrapper.Context;

        var codeListSample = new CodeListEntity
        {
            CodeListID = 0,
            Name = "codelistname"
        };

        databaseContext.CodeLists.Add(codeListSample);
        databaseContext.SaveChanges();

        var codeListItemSample = new CodeListItemEntity
        {
            CodeListItemID = 0,
            CodeListID = codeListSample.CodeListID,
            Value = "codelistvalue"
        };

        databaseContext.CodeListItems.Add(codeListItemSample);
        databaseContext.SaveChanges();

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

        databaseContext.Users.Add(userSample);
        databaseContext.SaveChanges();

        var folderSample = new FolderEntity
        {
            FolderID = 0,
            UserID = userSample.UserID,
            ParentID = default,
            TypeID = codeListItemSample.CodeListItemID,
            Name = "foldername",
            TotalSize = 3_000_000,
            TotalCount = 3
        };

        databaseContext.Folders.Add(folderSample);
        databaseContext.SaveChanges();

        var queryHandler = new GetFolderQueryHandler(databaseContext);
        var query = new GetFolderQuery(folderSample.FolderID);
        var cancellationToken = new CancellationToken();

        // Act.
        var originalFolder = queryHandler.Handle(query, cancellationToken).GetAwaiter().GetResult();

        // Assert.
        originalFolder.Should().NotBeNull();
    }

    [Fact]
    public void Handle_WhenEntityExistsNot_ThenReturnDefaultValue()
    {
        // Arrange.

        using var databaseContextWrapper = new CloudContextTestWrapper();

        databaseContextWrapper.Migrate();

        var databaseContext = databaseContextWrapper.Context;

        var codeListSample = new CodeListEntity
        {
            CodeListID = 0,
            Name = "codelistname"
        };

        databaseContext.CodeLists.Add(codeListSample);
        databaseContext.SaveChanges();

        var codeListItemSample = new CodeListItemEntity
        {
            CodeListItemID = 0,
            CodeListID = codeListSample.CodeListID,
            Value = "codelistvalue"
        };

        databaseContext.CodeListItems.Add(codeListItemSample);
        databaseContext.SaveChanges();

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

        databaseContext.Users.Add(userSample);
        databaseContext.SaveChanges();

        var folderSample = new FolderEntity
        {
            FolderID = 0,
            UserID = userSample.UserID,
            ParentID = default,
            TypeID = codeListItemSample.CodeListItemID,
            Name = "foldername",
            TotalSize = 3_000_000,
            TotalCount = 3
        };


        var queryHandler = new GetFolderQueryHandler(databaseContext);
        var query = new GetFolderQuery(folderSample.FolderID);
        var cancellationToken = new CancellationToken();

        // Act.
        var originalFolder = queryHandler.Handle(query, cancellationToken).GetAwaiter().GetResult();

        // Assert.
        originalFolder.Should().BeNull();
    }
}
