using AutoMapper;
using Linkerly.Data;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Commands;
using Linkerly.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Linkerly.Core.Application.Users.Commands;

public class UpdateUserCommand : ICommand<Unit>
{
    public UpdateUserCommand(int userID, string? identifier, string? email, bool hasEmailConfirmed, string? name, string? firstName, string? lastName, string? profilePhotoURL)
    {
        UserID = userID;
        Identifier = identifier;
        Email = email;
        HasEmailConfirmed = hasEmailConfirmed;
        Name = name;
        FirstName = firstName;
        LastName = lastName;
        ProfilePhotoURL = profilePhotoURL;
    }

    public int UserID { get; }
    public string? Identifier { get; }
    public string? Email { get; }
    public bool HasEmailConfirmed { get; }
    public string? Name { get; }
    public string? FirstName { get; }
    public string? LastName { get; }
    public string? ProfilePhotoURL { get; }
}

public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, Unit>
{
    private readonly CloudContext _databaseContext;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(CloudContext databaseContext, IMapper mapper)
    {
        _databaseContext = databaseContext;
        _mapper = mapper;
    }

    public Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        cancellationToken.ThrowIfCancellationRequested();

        UserEntity? originalUser = _databaseContext.Users.AsTracking().SingleOrDefault(user => user.UserID == request.UserID);

        if (originalUser is null)
        {
            throw new EntityNotFoundException(request.UserID.ToString(), nameof(UserEntity));
        }

        _ = _mapper.Map(request, originalUser);

        _ = _databaseContext.SaveChanges();

        return Unit.Task;
    }
}