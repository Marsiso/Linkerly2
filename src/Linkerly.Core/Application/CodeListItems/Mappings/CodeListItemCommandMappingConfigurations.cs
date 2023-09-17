using AutoMapper;
using Linkerly.Core.Application.CodeListItems.Commands;
using Linkerly.Domain.Application.Models;

namespace Linkerly.Core.Application.CodeListItems.Mappings;

public class CodeListItemCommandMappingConfigurations : Profile
{
    public CodeListItemCommandMappingConfigurations()
    {
        CreateMap<CodeListItemEntity, CreateCodeListItemCommand>().ReverseMap();
        CreateMap<CodeListItemEntity, UpdateCodeListItemCommand>().ReverseMap();
        CreateMap<CodeListItemEntity, DeleteCodeListItemCommand>().ReverseMap();
    }
}