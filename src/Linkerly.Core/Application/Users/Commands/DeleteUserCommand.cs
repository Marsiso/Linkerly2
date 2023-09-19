using Linkerly.Data;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Commands;
using Linkerly.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Linkerly.Core.Application.Users.Commands;

public class DeleteUserCommand : ICommand<Unit>
{
    public DeleteUserCommand(int userID)
    {
        UserID = userID;
    }

    public int UserID { get; }
}

public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, Unit>
{
    private readonly CloudContext _databaseContext;

    public DeleteUserCommandHandler(CloudContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        cancellationToken.ThrowIfCancellationRequested();

        var originalEntity = _databaseContext.Users
            .AsTracking()
            .SingleOrDefault(user => user.UserID == request.UserID);

        if (originalEntity is null) throw new EntityNotFoundException(request.UserID.ToString(), nameof(UserEntity));

        originalEntity.IsActive = false;

        _ = _databaseContext.Users.Update(originalEntity);

        _ = _databaseContext.SaveChanges();

        return Unit.Task;
    }
}
