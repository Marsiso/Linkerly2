using AutoMapper;
using Linkerly.Data;
using Linkerly.Domain.Application.Models;
using Linkerly.Domain.Commands;
using Linkerly.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Linkerly.Core.Application.AccessTokens.Commands;

public class UpdateAccessTokenCommand : ICommand<Unit>
{
	public UpdateAccessTokenCommand(int accessTokenID, int userId, string issuer, string subject, string audience, string issuedAt, string expires, string email, string isEmailVerified, string name, string? picture, string givenName, string familyName, string locale)
	{
		AccessTokenID = accessTokenID;
		UserID = userId;
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

	public int AccessTokenID { get; }
	public int UserID { get; set; }
	public string Issuer { get; }
	public string Subject { get; }
	public string Audience { get; }
	public string IssuedAt { get; }
	public string Expires { get; }
	public string Email { get; }
	public string IsEmailVerified { get; }
	public string Name { get; }
	public string? Picture { get; set; }
	public string GivenName { get; }
	public string FamilyName { get; }
	public string Locale { get; }
}

public class UpdateAccessTokenCommandHandler : ICommandHandler<UpdateAccessTokenCommand, Unit>
{
	private readonly CloudContext _databaseContext;
	private readonly IMapper _mapper;

	public UpdateAccessTokenCommandHandler(CloudContext databaseContext, IMapper mapper)
	{
		_databaseContext = databaseContext;
		_mapper = mapper;
	}

	public Task<Unit> Handle(UpdateAccessTokenCommand request, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(request);

		cancellationToken.ThrowIfCancellationRequested();

		var originalAccessToken = _databaseContext.AccessTokens.AsTracking().SingleOrDefault(token => token.AccessTokenID == request.AccessTokenID);

		if (originalAccessToken is null)
		{
			throw new EntityNotFoundException(request.AccessTokenID.ToString(), nameof(AccessTokenEntity));
		}

		_ = _mapper.Map(request, originalAccessToken);

		_ = _databaseContext.SaveChanges();

		return Unit.Task;
	}
}