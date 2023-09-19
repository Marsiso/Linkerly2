using AutoMapper;
using FluentAssertions;
using Linkerly.Core.Application.CodeLists.Commands;
using Linkerly.Core.Application.CodeLists.Mappings;
using Linkerly.Core.Tests.Helpers;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Exceptions;

namespace Linkerly.Core.Tests.Application.CodeLists.Commands;

public class UpdateCodeListCommandHandlerTestSuit
{
    [Fact]
    public void Handle_WhenRequestIsValid_ThenUpdateRecordInDatabase()
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

        _ = databaseContext.CodeLists.Add(codeListSample);
        _ = databaseContext.SaveChanges();

        codeListSample.Name = $"{codeListSample.Name}.{codeListSample.Name}";

        var commandHandler = new UpdateCodeListCommandHandler(databaseContext, mapper);
        var command = mapper.Map<UpdateCodeListCommand>(codeListSample);
        var cancellationToken = new CancellationToken();

        // Act.
        commandHandler.Handle(command, cancellationToken).GetAwaiter().GetResult();

        // Assert.
        databaseContext.CodeLists.Any(codeList => codeList.CodeListID == command.CodeListID).Should().BeTrue();
        databaseContext.CodeLists.SingleOrDefault(codeList => codeList.CodeListID == command.CodeListID)?.Name.Should().BeEquivalentTo(codeListSample.Name);
    }

    [Fact]
    public void Handle_WhenEntityNotFound_ThenThrowEntityNotFoundException()
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
            CodeListID = 1,
            Name = "codelistname"
        };

        var commandHandler = new UpdateCodeListCommandHandler(databaseContext, mapper);
        var command = mapper.Map<UpdateCodeListCommand>(codeListSample);
        var cancellationToken = new CancellationToken();

        // Act.
        Action action = () => commandHandler.Handle(command, cancellationToken).GetAwaiter().GetResult();

        var exception = Record.Exception(action);

        // Assert.
        exception.Should().NotBeNull();
        exception.Should().BeOfType<EntityNotFoundException>();

        (exception as EntityNotFoundException)?.EntityID.Should().BeEquivalentTo(codeListSample.CodeListID.ToString());
        (exception as EntityNotFoundException)?.EntityTypeName.Should().BeEquivalentTo(nameof(CodeListEntity));
    }
}
