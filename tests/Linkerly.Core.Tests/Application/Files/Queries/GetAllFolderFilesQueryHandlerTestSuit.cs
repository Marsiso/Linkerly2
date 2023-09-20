using FluentAssertions;
using Linkerly.Core.Application.Files.Queries;
using Linkerly.Core.Tests.Helpers;
using Linkerly.Domain.Application.Models;

namespace Linkerly.Core.Tests.Application.Files.Queries;

public class GetAllFolderFilesQueryHandlerTestSuit
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

        var folderSample = new FolderEntity
        {
            FolderID = 0,
            UserID = userSample.UserID,
            ParentID = default,
            TypeID = codeListItemSample.CodeListItemID,
            Name = "foldername",
            TotalSize = 3_000,
            TotalCount = 3
        };

        databaseContext.Folders.Add(folderSample);
        databaseContext.SaveChanges();

        var fileSamples = new List<FileEntity>
        {
            new()
            {
                FileID = 0,
                FolderID = folderSample.FolderID,
                ExtensionID = codeListItemSample.CodeListItemID,
                MimeTypeID = codeListItemSample.CodeListItemID,
                SafeName = "safename.copy.pdf",
                UnsafeName = "unsafename.copy.pdf",
                Location = Path.Combine(".\\", "Tests", "Temp"),
                Size = 1_000
            },
            new()
            {
                FileID = 0,
                FolderID = folderSample.FolderID,
                ExtensionID = codeListItemSample.CodeListItemID,
                MimeTypeID = codeListItemSample.CodeListItemID,
                SafeName = "safename.copy1.pdf",
                UnsafeName = "unsafename.copy1.pdf",
                Location = Path.Combine(".\\", "Tests", "Temp"),
                Size = 1_000
            },
            new()
            {
                FileID = 0,
                FolderID = folderSample.FolderID,
                ExtensionID = codeListItemSample.CodeListItemID,
                MimeTypeID = codeListItemSample.CodeListItemID,
                SafeName = "safename.copy2.pdf",
                UnsafeName = "unsafename.copy2.pdf",
                Location = Path.Combine(".\\", "Tests", "Temp"),
                Size = 1_000
            }
        };

        databaseContext.Files.AddRange(fileSamples);
        databaseContext.SaveChanges();

        var queryHandler = new GetAllFolderFilesQueryHandler(databaseContext);
        var query = new GetAllFolderFilesQuery(folderSample.FolderID);
        var cancellationToken = new CancellationToken();

        // Act.
        var originalFiles = queryHandler.Handle(query, cancellationToken).GetAwaiter().GetResult();

        // Assert.
        originalFiles.Should().NotBeNull();
        originalFiles.Should().NotBeEmpty();
        originalFiles.Should().HaveCount(fileSamples.Count);
    }

    [Fact]
    public void Handle_WhenEntitiesExistNot_ThenReturnDefaultValue()
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
            TotalSize = 3_000,
            TotalCount = 3
        };

        databaseContext.Folders.Add(folderSample);
        databaseContext.SaveChanges();

        var fileSamples = new List<FileEntity>
        {
            new()
            {
                FileID = 0,
                FolderID = folderSample.FolderID,
                ExtensionID = codeListItemSample.CodeListItemID,
                MimeTypeID = codeListItemSample.CodeListItemID,
                SafeName = "safename.copy.pdf",
                UnsafeName = "unsafename.copy.pdf",
                Location = Path.Combine(".\\", "Tests", "Temp"),
                Size = 1_000
            },
            new()
            {
                FileID = 0,
                FolderID = folderSample.FolderID,
                ExtensionID = codeListItemSample.CodeListItemID,
                MimeTypeID = codeListItemSample.CodeListItemID,
                SafeName = "safename.copy1.pdf",
                UnsafeName = "unsafename.copy1.pdf",
                Location = Path.Combine(".\\", "Tests", "Temp"),
                Size = 1_000
            },
            new()
            {
                FileID = 0,
                FolderID = folderSample.FolderID,
                ExtensionID = codeListItemSample.CodeListItemID,
                MimeTypeID = codeListItemSample.CodeListItemID,
                SafeName = "safename.copy2.pdf",
                UnsafeName = "unsafename.copy2.pdf",
                Location = Path.Combine(".\\", "Tests", "Temp"),
                Size = 1_000
            }
        };

        var queryHandler = new GetAllFolderFilesQueryHandler(databaseContext);
        var query = new GetAllFolderFilesQuery(folderSample.FolderID);
        var cancellationToken = new CancellationToken();

        // Act.
        var originalFiles = queryHandler.Handle(query, cancellationToken).GetAwaiter().GetResult();

        // Assert.
        originalFiles.Should().NotBeNull();
        originalFiles.Should().BeEmpty();
        originalFiles.Should().HaveCount(0);
    }
}
