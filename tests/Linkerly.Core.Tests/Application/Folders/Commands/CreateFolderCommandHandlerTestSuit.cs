using AutoMapper;
using FluentAssertions;
using Linkerly.Core.Application.Folders.Commands;
using Linkerly.Core.Application.Folders.Mappings;
using Linkerly.Core.Tests.Helpers;
using Linkerly.Domain.Application.Models;

namespace Linkerly.Core.Tests.Application.Folders.Commands;

public class CreateFolderCommandHandlerTestSuit
{
    [Fact]
    public void Handle_WhenRequestIsValid_ThenInsertRecordIntoDatabase()
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

        var commandHandler = new CreateFolderCommandHandler(databaseContext, mapper);
        var command = new CreateFolderCommand(userSample.UserID, default, codeListItemSample.CodeListItemID, "foldername", 1_000_000, 10);
        var cancellationToken = new CancellationToken();

        // Act.
        commandHandler.Handle(command, cancellationToken).GetAwaiter().GetResult();

        // Assert.
        databaseContext.Folders.Any().Should().BeTrue();
    }
}
