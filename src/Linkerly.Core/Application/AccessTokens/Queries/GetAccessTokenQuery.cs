using Linkerly.Data;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Queries;
using Microsoft.EntityFrameworkCore;

namespace Linkerly.Core.Application.AccessTokens.Queries;

public class GetAccessTokenQuery : IQuery<AccessTokenEntity?>
{
	public GetAccessTokenQuery(int accessTokenID)
	{
		AccessTokenID = accessTokenID;
	}

	public int AccessTokenID { get; }
}

public class GetAccessTokenQueryHandler : IQueryHandler<GetAccessTokenQuery, AccessTokenEntity?>
{
	private static readonly Func<CloudContext, int, AccessTokenEntity?> _query = EF.CompileQuery((CloudContext databaseContext, int accessTokenID) => databaseContext.AccessTokens.AsTracking().SingleOrDefault(token => token.AccessTokenID == accessTokenID));

	private readonly CloudContext _databaseContext;

	public GetAccessTokenQueryHandler(CloudContext databaseContext)
	{
		_databaseContext = databaseContext;
	}

	public Task<AccessTokenEntity?> Handle(GetAccessTokenQuery request, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(request);

		cancellationToken.ThrowIfCancellationRequested();

		if (request.AccessTokenID < 1)
		{
			return Task.FromResult<AccessTokenEntity?>(default);
		}

		var originalUser = _query(_databaseContext, request.AccessTokenID);

		return Task.FromResult(originalUser);
	}
}