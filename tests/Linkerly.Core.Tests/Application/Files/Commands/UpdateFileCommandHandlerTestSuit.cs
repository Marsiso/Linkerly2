using AutoMapper;
using FluentAssertions;
using Linkerly.Core.Application.Files.Commands;
using Linkerly.Core.Application.Files.Mappings;
using Linkerly.Core.Tests.Helpers;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Exceptions;

namespace Linkerly.Core.Tests.Application.Files.Commands;

public class UpdateFileCommandHandlerTestSuit
{
    [Fact]
    public void Handle_WhenRequestIsValid_ThenInsertRecordIntoDatabase()
    {
        // Arrange.
        var mappingProfile = new FileCommandMappingConfiguration();
        var mapperConfiguration = new MapperConfiguration(configuration => configuration.AddProfile(mappingProfile));
        var mapper = new Mapper(mapperConfiguration);

        using var databaseContextWrapper = new CloudContextTestWrapper();

        databaseContextWrapper.Migrate();

        var databaseContext = databaseContextWrapper.Context;

        var codeListSample = new CodeListEntity
        {
            Name = "codelistname"
        };

        _ = databaseContext.CodeLists.Add(codeListSample);
        _ = databaseContext.SaveChanges();

        var codeListItemSample = new CodeListItemEntity
        {
            CodeListID = codeListSample.CodeListID,
            Value = "codelistvalue"
        };

        _ = databaseContext.CodeListItems.Add(codeListItemSample);
        _ = databaseContext.SaveChanges();

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

        var folderSample = new FolderEntity
        {
            FolderID = 0,
            UserID = userSample.UserID,
            ParentID = default,
            TypeID = codeListItemSample.CodeListItemID,
            Name = "foldername",
            TotalSize = 1_000_000,
            TotalCount = 10
        };

        _ = databaseContext.Folders.Add(folderSample);
        _ = databaseContext.SaveChanges();

        var fileSample = new FileEntity
        {
            FileID = 0,
            FolderID = folderSample.FolderID,
            ExtensionID = codeListItemSample.CodeListItemID,
            MimeTypeID = codeListItemSample.CodeListItemID,
            SafeName = "safename.pdf",
            UnsafeName = "unsafename.pdf",
            Location = Path.Combine(".\\", "Tests", "Temp"),
            Size = 1_000
        };

        _ = databaseContext.Files.Add(fileSample);
        _ = databaseContext.SaveChanges();

        fileSample.UnsafeName = $"{fileSample.UnsafeName}.{fileSample.UnsafeName}";

        var commandHandler = new UpdateFileCommandHandler(databaseContext, mapper);
        var command = mapper.Map<UpdateFileCommand>(fileSample);
        var cancellationToken = new CancellationToken();

        // Act.
        commandHandler.Handle(command, cancellationToken).GetAwaiter().GetResult();

        // Assert.
        databaseContext.Files.Any(file => file.FileID == command.FileID).Should().BeTrue();
        databaseContext.Files.SingleOrDefault(folder => folder.FileID == command.FileID)?.UnsafeName.Should().BeEquivalentTo(fileSample.UnsafeName);
    }

    [Fact]
    public void Handle_WhenEntityNotFound_ThenThrowEntityNotFoundException()
    {
        // Arrange.
        var mappingProfile = new FileCommandMappingConfiguration();
        var mapperConfiguration = new MapperConfiguration(configuration => configuration.AddProfile(mappingProfile));
        var mapper = new Mapper(mapperConfiguration);

        using var databaseContextWrapper = new CloudContextTestWrapper();

        databaseContextWrapper.Migrate();

        var databaseContext = databaseContextWrapper.Context;

        var codeListSample = new CodeListEntity
        {
            Name = "codelistname"
        };

        _ = databaseContext.CodeLists.Add(codeListSample);
        _ = databaseContext.SaveChanges();

        var codeListItemSample = new CodeListItemEntity
        {
            CodeListID = codeListSample.CodeListID,
            Value = "codelistvalue"
        };

        _ = databaseContext.CodeListItems.Add(codeListItemSample);
        _ = databaseContext.SaveChanges();

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

        var folderSample = new FolderEntity
        {
            FolderID = 0,
            UserID = userSample.UserID,
            ParentID = default,
            TypeID = codeListItemSample.CodeListItemID,
            Name = "foldername",
            TotalSize = 1_000_000,
            TotalCount = 10
        };

        _ = databaseContext.Folders.Add(folderSample);
        _ = databaseContext.SaveChanges();

        var fileSample = new FileEntity
        {
            FileID = 1,
            FolderID = folderSample.FolderID,
            ExtensionID = codeListItemSample.CodeListItemID,
            MimeTypeID = codeListItemSample.CodeListItemID,
            SafeName = "safename.pdf",
            UnsafeName = "unsafename.pdf",
            Location = Path.Combine(".\\", "Tests", "Temp"),
            Size = 1_000
        };

        var commandHandler = new UpdateFileCommandHandler(databaseContext, mapper);
        var command = mapper.Map<UpdateFileCommand>(fileSample);
        var cancellationToken = new CancellationToken();

        // Act.
        Action action = () => commandHandler.Handle(command, cancellationToken).GetAwaiter().GetResult();

        var exception = Record.Exception(action);

        // Assert.
        exception.Should().NotBeNull();
        exception.Should().BeOfType<EntityNotFoundException>();

        (exception as EntityNotFoundException)?.EntityID.Should().BeEquivalentTo(fileSample.FileID.ToString());
        (exception as EntityNotFoundException)?.EntityTypeName.Should().BeEquivalentTo(nameof(FileEntity));
    }
}
