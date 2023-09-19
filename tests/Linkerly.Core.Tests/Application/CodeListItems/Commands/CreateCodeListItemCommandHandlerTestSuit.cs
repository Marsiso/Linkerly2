using AutoMapper;
using FluentAssertions;
using Linkerly.Core.Application.CodeListItems.Commands;
using Linkerly.Core.Application.CodeListItems.Mappings;
using Linkerly.Core.Tests.Helpers;
using Linkerly.Domain.Application.Models;

namespace Linkerly.Core.Tests.Application.CodeListItems.Commands;

public class CreateCodeListItemCommandHandlerTestSuit
{
    [Fact]
    public void Handle_WhenRequestIsValid_ThenInsertRecordIntoDatabase()
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

        var commandHandler = new CreateCodeListItemCommandHandler(databaseContext, mapper);
        var command = mapper.Map<CreateCodeListItemCommand>(codeListItemSample);
        var cancellationToken = new CancellationToken();

        // Act.
        commandHandler.Handle(command, cancellationToken).GetAwaiter().GetResult();

        // Assert.
        databaseContext.CodeListItems.Any().Should().BeTrue();
    }
}
