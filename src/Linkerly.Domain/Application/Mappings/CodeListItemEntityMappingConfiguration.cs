using AutoMapper;
using Linkerly.Domain.Application.Models;

namespace Linkerly.Domain.Application.Mappings;

public class CodeListItemEntityMappingConfiguration : Profile
{
	public CodeListItemEntityMappingConfiguration()
	{
		CreateMap<CodeListItemEntity, CodeListItemEntity>()
			.ForMember(user => user.CodeList, options => options.Ignore())
			.ForMember(user => user.UserCreatedBy, options => options.Ignore())
			.ForMember(user => user.UserUpdatedBy, options => options.Ignore());
	}
}