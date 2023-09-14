using Linkerly.Data;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Queries;
using Microsoft.EntityFrameworkCore;

namespace Linkerly.Core.Application.Users.Queries;

public class GetUserBySubjectQuery : IQuery<UserEntity>
{
	public GetUserBySubjectQuery(string? subject)
	{
		Subject = subject;
	}

	public string? Subject { get; }
}

public class GetUserBySubjectQueryHandler : IQueryHandler<GetUserBySubjectQuery, UserEntity?>
{
	private static readonly Func<CloudContext, string, UserEntity?> _query = EF.CompileQuery((CloudContext databaseContext, string subject) => databaseContext.Users.AsTracking().SingleOrDefault(user => user.AccessToken!.Subject == subject));

	private readonly CloudContext _databaseContext;

	public GetUserBySubjectQueryHandler(CloudContext databaseContext)
	{
		_databaseContext = databaseContext;
	}

	public Task<UserEntity?> Handle(GetUserBySubjectQuery request, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(request);

		cancellationToken.ThrowIfCancellationRequested();

		if (string.IsNullOrWhiteSpace(request.Subject))
		{
			return Task.FromResult<UserEntity?>(default);
		}

		var originalUser = _query(_databaseContext, request.Subject);

		return Task.FromResult(originalUser);
	}
}