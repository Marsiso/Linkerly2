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
    public UpdateUserCommand(int userID, string? identifier, string? email, bool hasEmailConfirmed, string? name, string? givenName, string? familyName, string? picture, string? locale)
    {
        UserID = userID;
        Identifier = identifier;
        Email = email;
        HasEmailConfirmed = hasEmailConfirmed;
        Name = name;
        GivenName = givenName;
        FamilyName = familyName;
        Picture = picture;
        Locale = locale;
    }

    public int UserID { get; }
    public string? Identifier { get; }
    public string? Email { get; }
    public bool HasEmailConfirmed { get; }
    public string? Name { get; }
    public string? GivenName { get; }
    public string? FamilyName { get; }
    public string? Picture { get; }
    public string? Locale { get; }
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

        var originalUser = _databaseContext.Users
            .AsTracking()
            .SingleOrDefault(user => user.UserID == request.UserID);

        if (originalUser is null) throw new EntityNotFoundException(request.UserID.ToString(), nameof(UserEntity));

        _ = _mapper.Map(request, originalUser);

        _ = _databaseContext.SaveChanges();

        return Unit.Task;
    }
}
