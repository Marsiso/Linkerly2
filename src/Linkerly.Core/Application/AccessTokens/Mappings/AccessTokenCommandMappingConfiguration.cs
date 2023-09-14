using AutoMapper;
using Linkerly.Core.Application.AccessTokens.Commands;
using Linkerly.Domain.Application.Models;

namespace Linkerly.Core.Application.AccessTokens.Mappings;

public class AccessTokenCommandMappingConfiguration : Profile
{
	public AccessTokenCommandMappingConfiguration()
	{
		CreateMap<AccessTokenEntity, CreateAccessTokenCommand>().ReverseMap();
		CreateMap<AccessTokenEntity, UpdateAccessTokenCommand>().ReverseMap();
		CreateMap<AccessTokenEntity, DeleteAccessTokenCommand>().ReverseMap();
	}
}