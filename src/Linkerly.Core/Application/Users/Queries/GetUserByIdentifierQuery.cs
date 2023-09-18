using Linkerly.Data;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Queries;
using Microsoft.EntityFrameworkCore;

namespace Linkerly.Core.Application.Users.Queries;

public class GetUserByIdentifierQuery : IQuery<UserEntity?>
{
    public GetUserByIdentifierQuery(string? identifier)
    {
        Identifier = identifier;
    }

    public string? Identifier { get; }
}

public class GetUserByIdentifierQueryHandler : IQueryHandler<GetUserByIdentifierQuery, UserEntity?>
{
    private static readonly Func<CloudContext, string, UserEntity?> _query = EF.CompileQuery((CloudContext databaseContext, string subject) =>
        databaseContext.Users
            .AsNoTracking()
            .SingleOrDefault(user => user.Identifier == subject));

    private readonly CloudContext _databaseContext;

    public GetUserByIdentifierQueryHandler(CloudContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public Task<UserEntity?> Handle(GetUserByIdentifierQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(request.Identifier)) return Task.FromResult<UserEntity?>(default);

        var originalUser = _query(_databaseContext, request.Identifier);

        return Task.FromResult(originalUser);
    }
}
