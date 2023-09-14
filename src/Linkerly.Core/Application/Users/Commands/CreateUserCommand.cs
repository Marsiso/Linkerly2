using AutoMapper;
using Linkerly.Data;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Commands;
using MediatR;

namespace Linkerly.Core.Application.Users.Commands;

public class CreateUserCommand : ICommand<Unit>
{
	public CreateUserCommand(string? email, string? firstName, string? lastName, string? profilePhotoUrl, DateTime? dateLastAccessed)
	{
		Email = email;
		FirstName = firstName;
		LastName = lastName;
		ProfilePhotoUrl = profilePhotoUrl;
		DateLastAccessed = dateLastAccessed;
	}

	public string? Email { get; }
	public string? FirstName { get; }
	public string? LastName { get; }
	public string? ProfilePhotoUrl { get; }
	public DateTime? DateLastAccessed { get; }
}

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Unit>
{
	private readonly CloudContext _databaseContext;
	private readonly IMapper _mapper;

	public CreateUserCommandHandler(CloudContext databaseContext, IMapper mapper)
	{
		_databaseContext = databaseContext;
		_mapper = mapper;
	}

	public Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(request);

		cancellationToken.ThrowIfCancellationRequested();

		var userToCreate = _mapper.Map<UserEntity>(request);

		_ = _databaseContext.Users.Add(userToCreate);

		_ = _databaseContext.SaveChanges();

		return Unit.Task;
	}
}