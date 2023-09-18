using AutoMapper;
using Linkerly.Core.Application.Folders.Commands;
using Linkerly.Domain.Application.Models;

namespace Linkerly.Core.Application.Folders.Mappings;

public class FolderCommandMappingConfiguration : Profile
{
    public FolderCommandMappingConfiguration()
    {
        CreateMap<FolderEntity, CreateFolderCommand>().ReverseMap();
        CreateMap<FolderEntity, UpdateFolderCommand>().ReverseMap();
        CreateMap<FolderEntity, DeleteFolderCommand>().ReverseMap();
    }
}
