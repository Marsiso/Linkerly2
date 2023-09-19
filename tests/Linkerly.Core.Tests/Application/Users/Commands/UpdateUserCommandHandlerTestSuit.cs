﻿using AutoMapper;
using FluentAssertions;
using Linkerly.Core.Application.Users.Commands;
using Linkerly.Core.Application.Users.Mappings;
using Linkerly.Core.Tests.Helpers;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Exceptions;

namespace Linkerly.Core.Tests.Application.Users.Commands;

public class UpdateUserCommandHandlerTestSuit
{
    [Fact]
    public void Handle_WhenRequestIsValid_ThenUpdateRecordInDatabase()
    {
        // Arrange.
        var mappingProfile = new UserCommandMappingConfiguration();
        var mapperConfiguration = new MapperConfiguration(configuration => configuration.AddProfile(mappingProfile));
        var mapper = new Mapper(mapperConfiguration);

        using var databaseContextWrapper = new CloudContextTestWrapper();

        databaseContextWrapper.Migrate();

        var databaseContext = databaseContextWrapper.Context;

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

        userSample.Name = $"{userSample.Name}.{userSample.Name}";

        var commandHandler = new UpdateUserCommandHandler(databaseContext, mapper);
        var command = mapper.Map<UpdateUserCommand>(userSample);
        var cancellationToken = new CancellationToken();

        // Act.
        commandHandler.Handle(command, cancellationToken).GetAwaiter().GetResult();


        // Assert.
        databaseContext.Users.Any(user => user.UserID == command.UserID).Should().BeTrue();
        databaseContext.Users.SingleOrDefault(user => user.UserID == command.UserID)?.Email.Should().BeEquivalentTo(command.Email);
    }

    [Fact]
    public void Handle_WhenEntityNotFound_ThenThrowEntityNotFoundException()
    {
        // Arrange.
        var mappingProfile = new UserCommandMappingConfiguration();
        var mapperConfiguration = new MapperConfiguration(configuration => configuration.AddProfile(mappingProfile));
        var mapper = new Mapper(mapperConfiguration);

        using var databaseContextWrapper = new CloudContextTestWrapper();

        databaseContextWrapper.Migrate();

        var databaseContext = databaseContextWrapper.Context;

        var userSample = new UserEntity
        {
            UserID = 1,
            Identifier = "123456789",
            Email = "givenname.familyname@example.com",
            HasEmailConfirmed = true,
            Name = "givenname \"nickanme\" familyname",
            GivenName = "givenname",
            FamilyName = "familyname",
            Picture = default,
            Locale = "cs"
        };

        var commandHandler = new UpdateUserCommandHandler(databaseContext, mapper);
        var command = mapper.Map<UpdateUserCommand>(userSample);
        var cancellationToken = new CancellationToken();

        // Act.
        Action action = () => commandHandler.Handle(command, cancellationToken).GetAwaiter().GetResult();

        var exception = Record.Exception(action);

        // Assert.
        exception.Should().NotBeNull();
        exception.Should().BeOfType<EntityNotFoundException>();

        (exception as EntityNotFoundException)?.EntityID.Should().BeEquivalentTo("1");
        (exception as EntityNotFoundException)?.EntityTypeName.Should().BeEquivalentTo(nameof(UserEntity));
    }
}
