using Linkerly.Data;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Queries;
using Microsoft.EntityFrameworkCore;

namespace Linkerly.Core.Application.Users.Queries;

public class GetUserQuery : IQuery<UserEntity?>
{
    public GetUserQuery(int userId)
    {
        UserID = userId;
    }

    public int UserID { get; }
}

public class GetUserQueryHandler : IQueryHandler<GetUserQuery, UserEntity?>
{
    private static readonly Func<CloudContext, int, UserEntity?> _query = EF.CompileQuery((CloudContext databaseContext, int userID) => databaseContext.Users.AsTracking().SingleOrDefault(user => user.UserID == userID));

    private readonly CloudContext _databaseContext;

    public GetUserQueryHandler(CloudContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public Task<UserEntity?> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        cancellationToken.ThrowIfCancellationRequested();

        if (request.UserID < 1)
        {
            return Task.FromResult<UserEntity?>(default);
        }

        UserEntity? originalUser = _query(_databaseContext, request.UserID);

        return Task.FromResult(originalUser);
    }
}