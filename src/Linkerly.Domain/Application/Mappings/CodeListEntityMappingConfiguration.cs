using AutoMapper;
using Linkerly.Domain.Application.Models;

namespace Linkerly.Domain.Application.Mappings;

public class CodeListEntityMappingConfiguration : Profile
{
	public CodeListEntityMappingConfiguration()
	{
		CreateMap<CodeListEntity, CodeListEntity>()
			.ForMember(user => user.Items, options => options.Ignore())
			.ForMember(user => user.UserCreatedBy, options => options.Ignore())
			.ForMember(user => user.UserUpdatedBy, options => options.Ignore());
	}
}