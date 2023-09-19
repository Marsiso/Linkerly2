using AutoMapper;
using FluentAssertions;
using Linkerly.Core.Application.CodeLists.Commands;
using Linkerly.Core.Application.CodeLists.Mappings;
using Linkerly.Core.Tests.Helpers;
using Linkerly.Domain.Application.Models;

namespace Linkerly.Core.Tests.Application.CodeLists.Commands;

public class CreateCodeListCommandHandlerTestSuit
{
    [Fact]
    public void Handle_WhenRequestIsValid_ThenInsertRecordIntoDatabase()
    {
        // Arrange.
        var mappingProfile = new CodeListCommandMappingConfigurations();
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

        var commandHandler = new CreateCodeListCommandHandler(databaseContext, mapper);
        var command = mapper.Map<CreateCodeListCommand>(codeListSample);
        var cancellationToken = new CancellationToken();

        // Act.
        commandHandler.Handle(command, cancellationToken).GetAwaiter().GetResult();

        // Assert.
        databaseContext.CodeLists.Any().Should().BeTrue();
    }
}
