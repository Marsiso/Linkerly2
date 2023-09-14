using AutoMapper;
using Linkerly.Data;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Commands;
using MediatR;

namespace Linkerly.Core.Application.AccessTokens.Commands;

public class CreateAccessTokenCommand : ICommand<Unit>
{
	public CreateAccessTokenCommand(int userID, string? issuer, string? subject, string? audience, string? issuedAt, string? expires, string? email, string? isEmailVerified, string? name, string? picture, string givenName, string? familyName, string? locale)
	{
		UserID = userID;
		Issuer = issuer;
		Subject = subject;
		Audience = audience;
		IssuedAt = issuedAt;
		Expires = expires;
		Email = email;
		IsEmailVerified = isEmailVerified;
		Name = name;
		Picture = picture;
		GivenName = givenName;
		FamilyName = familyName;
		Locale = locale;
	}

	public int UserID { get; }
	public string? Issuer { get; }
	public string? Subject { get; }
	public string? Audience { get; }
	public string? IssuedAt { get; }
	public string? Expires { get; }
	public string? Email { get; }
	public string? IsEmailVerified { get; }
	public string? Name { get; }
	public string? Picture { get; }
	public string GivenName { get; }
	public string? FamilyName { get; }
	public string? Locale { get; }
}

public class CreateAccessTokenCommandHandler : ICommandHandler<CreateAccessTokenCommand, Unit>
{
	private readonly CloudContext _databaseContext;
	private readonly IMapper _mapper;

	public CreateAccessTokenCommandHandler(CloudContext databaseContext, IMapper mapper)
	{
		_databaseContext = databaseContext;
		_mapper = mapper;
	}

	public Task<Unit> Handle(CreateAccessTokenCommand request, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(request);

		cancellationToken.ThrowIfCancellationRequested();

		var accessTokenToCreate = _mapper.Map<AccessTokenEntity>(request);

		_ = _databaseContext.AccessTokens.Add(accessTokenToCreate);

		_ = _databaseContext.SaveChanges();

		return Unit.Task;
	}
}