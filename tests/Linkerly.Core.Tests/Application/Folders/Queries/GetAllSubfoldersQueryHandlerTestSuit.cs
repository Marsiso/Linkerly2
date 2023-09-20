using FluentAssertions;
using Linkerly.Core.Application.Folders.Queries;
using Linkerly.Core.Tests.Helpers;
using Linkerly.Domain.Application.Models;

namespace Linkerly.Core.Tests.Application.Folders.Queries;

public class GetAllSubfoldersQueryHandlerTestSuit
{
    [Fact]
    public void Handle_WhenEntitiesExist_ThenReturnValue()
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

        var rootFolderSample = new FolderEntity
        {
            FolderID = 0,
            UserID = userSample.UserID,
            ParentID = default,
            TypeID = codeListItemSample.CodeListItemID,
            Name = "foldername",
            TotalSize = 3_000_000,
            TotalCount = 3
        };

        databaseContext.Folders.Add(rootFolderSample);
        databaseContext.SaveChanges();

        // TODO Fix: 1 - 1 relationship between folders and user
        var subfoldersSample = new List<FolderEntity>
        {
            new()
            {
                FolderID = 0,
                UserID = userSample.UserID,
                ParentID = rootFolderSample.FolderID,
                TypeID = codeListItemSample.CodeListItemID,
                Name = $"{rootFolderSample.Name}.copy",
                TotalSize = 1_000_000,
                TotalCount = 10
            },
            new()
            {
                FolderID = 0,
                UserID = userSample.UserID,
                ParentID = rootFolderSample.FolderID,
                TypeID = codeListItemSample.CodeListItemID,
                Name = $"{rootFolderSample.Name}.copy1",
                TotalSize = 1_000_000,
                TotalCount = 10
            },
            new()
            {
                FolderID = 0,
                UserID = userSample.UserID,
                ParentID = rootFolderSample.FolderID,
                TypeID = codeListItemSample.CodeListItemID,
                Name = $"{rootFolderSample.Name}.copy2",
                TotalSize = 1_000_000,
                TotalCount = 10
            }
        };

        databaseContext.Folders.AddRange(subfoldersSample);
        databaseContext.SaveChanges();

        var queryHandler = new GetAllSubfoldersQueryHandler(databaseContext);
        var query = new GetAllSubfoldersQuery(rootFolderSample.FolderID);
        var cancellationToken = new CancellationToken();

        // Act.
        var subfolders = queryHandler.Handle(query, cancellationToken).GetAwaiter().GetResult();

        // Assert.
        subfolders.Should().NotBeNull();
        subfolders.Should().NotBeEmpty();
        subfolders.Should().HaveCount(subfoldersSample.Count);
    }
}
