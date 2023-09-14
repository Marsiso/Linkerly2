using Linkerly.Data;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Queries;
using Microsoft.EntityFrameworkCore;

namespace Linkerly.Core.Application.Users.Queries;

public class GetUserByEmailQuery : IQuery<UserEntity?>
{
	public GetUserByEmailQuery(string? email)
	{
		Email = email;
	}

	public string? Email { get; }
}

public class GetUserByEmailQueryHandler : IQueryHandler<GetUserByEmailQuery, UserEntity?>
{
	private static readonly Func<CloudContext, string, UserEntity?> _query = EF.CompileQuery((CloudContext databaseContext, string email) => databaseContext.Users.AsTracking().SingleOrDefault(user => user.Email == email));

	private readonly CloudContext _databaseContext;

	public GetUserByEmailQueryHandler(CloudContext databaseContext)
	{
		_databaseContext = databaseContext;
	}

	public Task<UserEntity?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(request);

		cancellationToken.ThrowIfCancellationRequested();

		if (string.IsNullOrWhiteSpace(request.Email))
		{
			return Task.FromResult<UserEntity?>(default);
		}

		var originalUser = _query(_databaseContext, request.Email);

		return Task.FromResult(originalUser);
	}
}