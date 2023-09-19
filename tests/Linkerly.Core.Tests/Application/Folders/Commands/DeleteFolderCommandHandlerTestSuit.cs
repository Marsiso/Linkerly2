using AutoMapper;
using FluentAssertions;
using Linkerly.Core.Application.Folders.Commands;
using Linkerly.Core.Application.Folders.Mappings;
using Linkerly.Core.Tests.Helpers;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Linkerly.Core.Tests.Application.Folders.Commands;

public class DeleteFolderCommandHandlerTestSuit
{
    [Fact]
    public void Handle_WhenRequestIsValid_ThenRemoveRecordFromDatabase()
    {
        // Arrange.
        var mappingProfile = new FolderCommandMappingConfiguration();
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

        var commandHandler = new DeleteFolderCommandHandler(databaseContext);
        var command = mapper.Map<DeleteFolderCommand>(folderSample);
        var cancellationToken = new CancellationToken();

        // Act.
        commandHandler.Handle(command, cancellationToken).GetAwaiter().GetResult();

        // Assert.
        databaseContext.Folders.AsNoTracking().Any(folder => folder.FolderID == command.FolderID).Should().BeFalse();
        databaseContext.Folders.AsNoTracking().IgnoreQueryFilters().Any(folder => folder.FolderID == command.FolderID).Should().BeTrue();
        databaseContext.Folders.AsNoTracking().IgnoreQueryFilters().SingleOrDefault(folder => folder.FolderID == command.FolderID)?.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Handle_WhenEntityNotFound_ThenThrowEntityNotFoundException()
    {
        // Arrange.
        var mappingProfile = new FolderCommandMappingConfiguration();
        var mapperConfiguration = new MapperConfiguration(configuration => configuration.AddProfile(mappingProfile));
        var mapper = new Mapper(mapperConfiguration);

        using var databaseContextWrapper = new CloudContextTestWrapper();

        databaseContextWrapper.Migrate();

        var databaseContext = databaseContextWrapper.Context;

        var codeListSample = new CodeListEntity
        {
            CodeListID = 0,
            Name = "codelistname"
        };

        _ = databaseContext.CodeLists.Add(codeListSample);
        _ = databaseContext.SaveChanges();

        var codeListItemSample = new CodeListItemEntity
        {
            CodeListItemID = 0,
            CodeListID = codeListSample.CodeListID,
            Value = "codelistvalue"
        };

        _ = databaseContext.CodeListItems.Add(codeListItemSample);
        _ = databaseContext.SaveChanges();

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

        var folderSample = new FolderEntity
        {
            FolderID = 1,
            UserID = userSample.UserID,
            ParentID = default,
            TypeID = codeListItemSample.CodeListItemID,
            Name = "foldername",
            TotalSize = 1_000_000,
            TotalCount = 10
        };

        var commandHandler = new DeleteFolderCommandHandler(databaseContext);
        var command = mapper.Map<DeleteFolderCommand>(folderSample);
        var cancellationToken = new CancellationToken();

        // Act.
        Action action = () => commandHandler.Handle(command, cancellationToken).GetAwaiter().GetResult();

        var exception = Record.Exception(action);

        // Assert.
        exception.Should().NotBeNull();
        exception.Should().BeOfType<EntityNotFoundException>();

        (exception as EntityNotFoundException)?.EntityID.Should().BeEquivalentTo(folderSample.FolderID.ToString());
        (exception as EntityNotFoundException)?.EntityTypeName.Should().BeEquivalentTo(nameof(FolderEntity));
    }
}
