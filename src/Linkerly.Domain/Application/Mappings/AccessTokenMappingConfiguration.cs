using AutoMapper;

namespace Linkerly.Domain.Application.Mappings;

public class AccessTokenMappingConfiguration : Profile
{
	public AccessTokenMappingConfiguration()
	{
		CreateMap<AccessTokenEntity, AccessTokenEntity>()
			.ForMember(token => token.UserCreatedBy, options => options.Ignore())
			.ForMember(token => token.UserUpdatedBy, options => options.Ignore());
	}
}