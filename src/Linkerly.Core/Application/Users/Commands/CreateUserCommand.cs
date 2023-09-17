using AutoMapper;
using Linkerly.Data;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Commands;
using MediatR;

namespace Linkerly.Core.Application.Users.Commands;

public class CreateUserCommand : ICommand<Unit>
{
    public CreateUserCommand(string? identifier, string? email, bool hasEmailConfirmed, string? name, string? firstName, string? lastName, string? profilePhotoUrl)
    {
        Identifier = identifier;
        Email = email;
        HasEmailConfirmed = hasEmailConfirmed;
        Name = name;
        FirstName = firstName;
        LastName = lastName;
        ProfilePhotoURL = profilePhotoUrl;
    }

    public string? Identifier { get; }
    public string? Email { get; }
    public bool HasEmailConfirmed { get; }
    public string? Name { get; }
    public string? FirstName { get; }
    public string? LastName { get; }
    public string? ProfilePhotoURL { get; }
}

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Unit>
{
    private readonly CloudContext _databaseContext;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(CloudContext databaseContext, IMapper mapper)
    {
        _databaseContext = databaseContext;
        _mapper = mapper;
    }

    public Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        cancellationToken.ThrowIfCancellationRequested();

        UserEntity? userToCreate = _mapper.Map<UserEntity>(request);

        _ = _databaseContext.Users.Add(userToCreate);

        _ = _databaseContext.SaveChanges();

        return Unit.Task;
    }
}