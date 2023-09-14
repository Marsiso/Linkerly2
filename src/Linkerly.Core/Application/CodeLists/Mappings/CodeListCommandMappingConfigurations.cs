using AutoMapper;
using Linkerly.Core.Application.CodeLists.Commands;
using Linkerly.Domain.Application.Models;

namespace Linkerly.Core.Application.CodeLists.Mappings;

public class CodeListCommandMappingConfigurations : Profile
{
	public CodeListCommandMappingConfigurations()
	{
		CreateMap<CodeListEntity, CreateCodeListCommand>().ReverseMap();
		CreateMap<CodeListEntity, UpdateCodeListCommand>().ReverseMap();
		CreateMap<CodeListEntity, DeleteCodeListCommand>().ReverseMap();
	}
}