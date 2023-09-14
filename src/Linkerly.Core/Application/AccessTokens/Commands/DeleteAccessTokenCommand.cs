using AutoMapper;
using Linkerly.Data;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Commands;
using Linkerly.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Linkerly.Core.Application.AccessTokens.Commands;

public class DeleteAccessTokenCommand : ICommand<Unit>
{
	public DeleteAccessTokenCommand(int accessTokenID)
	{
		AccessTokenID = accessTokenID;
	}

	public int AccessTokenID { get; }
}

public class DeleteAccessTokenCommandHandler : ICommandHandler<DeleteAccessTokenCommand, Unit>
{
	private readonly CloudContext _databaseContext;
	private readonly IMapper _mapper;

	public DeleteAccessTokenCommandHandler(CloudContext databaseContext, IMapper mapper)
	{
		_databaseContext = databaseContext;
		_mapper = mapper;
	}

	public Task<Unit> Handle(DeleteAccessTokenCommand request, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(request);

		cancellationToken.ThrowIfCancellationRequested();

		var originalAccessToken = _databaseContext.AccessTokens.AsTracking().SingleOrDefault(token => token.AccessTokenID == request.AccessTokenID);

		if (originalAccessToken is null)
		{
			throw new EntityNotFoundException(request.AccessTokenID.ToString(), nameof(AccessTokenEntity));
		}

		originalAccessToken.IsActive = false;

		_ = _databaseContext.SaveChanges();

		return Unit.Task;
	}
}