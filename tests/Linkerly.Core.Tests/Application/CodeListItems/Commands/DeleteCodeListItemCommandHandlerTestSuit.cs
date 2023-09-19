using AutoMapper;
using FluentAssertions;
using Linkerly.Core.Application.CodeListItems.Commands;
using Linkerly.Core.Application.CodeListItems.Mappings;
using Linkerly.Core.Tests.Helpers;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Linkerly.Core.Tests.Application.CodeListItems.Commands;

public class DeleteCodeListItemCommandHandlerTestSuit
{
    [Fact]
    public void Handle_WhenRequestIsValid_ThenRemoveRecordFromDatabase()
    {
        // Arrange.
        var mappingProfile = new CodeListItemCommandMappingConfigurations();
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

        var commandHandler = new DeleteCodeListItemCommandHandler(databaseContext);
        var command = mapper.Map<DeleteCodeListItemCommand>(codeListItemSample);
        var cancellationToken = new CancellationToken();

        // Act.
        commandHandler.Handle(command, cancellationToken).GetAwaiter().GetResult();

        // Assert.
        databaseContext.CodeListItems.AsNoTracking().Any(codeListItem => codeListItem.CodeListItemID == command.CodeListItemID).Should().BeFalse();
        databaseContext.CodeListItems.AsNoTracking().IgnoreQueryFilters().Any(codeListItem => codeListItem.CodeListItemID == command.CodeListItemID).Should().BeTrue();
        databaseContext.CodeListItems.AsNoTracking().IgnoreQueryFilters().SingleOrDefault(codeListItem => codeListItem.CodeListItemID == command.CodeListItemID)?.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Handle_WhenEntityNotFound_ThenThrowEntityNotFoundException()
    {
        // Arrange.
        var mappingProfile = new CodeListItemCommandMappingConfigurations();
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
            CodeListItemID = 1,
            CodeListID = codeListSample.CodeListID,
            Value = "codelistvalue"
        };

        var commandHandler = new DeleteCodeListItemCommandHandler(databaseContext);
        var command = mapper.Map<DeleteCodeListItemCommand>(codeListItemSample);
        var cancellationToken = new CancellationToken();

        // Act.
        Action action = () => commandHandler.Handle(command, cancellationToken).GetAwaiter().GetResult();

        var exception = Record.Exception(action);

        // Assert.
        exception.Should().NotBeNull();
        exception.Should().BeOfType<EntityNotFoundException>();

        (exception as EntityNotFoundException)?.EntityID.Should().BeEquivalentTo(codeListItemSample.CodeListItemID.ToString());
        (exception as EntityNotFoundException)?.EntityTypeName.Should().BeEquivalentTo(nameof(CodeListItemEntity));
    }
}
