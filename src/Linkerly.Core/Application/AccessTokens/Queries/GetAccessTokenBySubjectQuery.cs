using Linkerly.Data;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Queries;
using Microsoft.EntityFrameworkCore;

namespace Linkerly.Core.Application.AccessTokens.Queries;

public class GetAccessTokenBySubjectQuery : IQuery<AccessTokenEntity>
{
	public GetAccessTokenBySubjectQuery(string? subject)
	{
		Subject = subject;
	}

	public string? Subject { get; }
}

public class GetAccessTokenBySubjectQueryHandler : IQueryHandler<GetAccessTokenBySubjectQuery, AccessTokenEntity?>
{
	private static readonly Func<CloudContext, string, AccessTokenEntity?> _query = EF.CompileQuery((CloudContext databaseContext, string subject) => databaseContext.AccessTokens.AsTracking().SingleOrDefault(token => token.Subject == subject));

	private readonly CloudContext _databaseContext;

	public GetAccessTokenBySubjectQueryHandler(CloudContext databaseContext)
	{
		_databaseContext = databaseContext;
	}

	public Task<AccessTokenEntity?> Handle(GetAccessTokenBySubjectQuery request, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(request);

		cancellationToken.ThrowIfCancellationRequested();

		if (string.IsNullOrWhiteSpace(request.Subject))
		{
			return Task.FromResult<AccessTokenEntity?>(default);
		}

		var originalAccessToken = _query(_databaseContext, request.Subject);

		return Task.FromResult(originalAccessToken);
	}
}