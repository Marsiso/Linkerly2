﻿using AutoMapper;
using Linkerly.Data;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Commands;
using Linkerly.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Linkerly.Core.Application.Users.Commands;

public class UpdateUserCommand : ICommand<Unit>
{
	public UpdateUserCommand(int userId, string? email, string? firstName, string? lastName, string? profilePhotoUrl, DateTime? dateLastAccessed)
	{
		UserID = userId;
		Email = email;
		FirstName = firstName;
		LastName = lastName;
		ProfilePhotoUrl = profilePhotoUrl;
		DateLastAccessed = dateLastAccessed;
	}

	public int UserID { get; }
	public string? Email { get; }
	public string? FirstName { get; }
	public string? LastName { get; }
	public string? ProfilePhotoUrl { get; }
	public DateTime? DateLastAccessed { get; }
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

		var originalEntity = _databaseContext.Users.AsTracking().SingleOrDefault(user => user.UserID == request.UserID);

		if (originalEntity is null)
		{
			throw new EntityNotFoundException(request.UserID.ToString(), nameof(UserEntity));
		}

		_ = _mapper.Map(request, originalEntity);

		_ = _databaseContext.SaveChanges();

		return Unit.Task;
	}
}