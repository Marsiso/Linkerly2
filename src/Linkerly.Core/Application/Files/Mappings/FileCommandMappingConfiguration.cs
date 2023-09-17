using AutoMapper;
using Linkerly.Core.Application.Files.Commands;
using Linkerly.Domain.Application.Models;

namespace Linkerly.Core.Application.Files.Mappings;

public class FileCommandMappingConfiguration : Profile
{
    public FileCommandMappingConfiguration()
    {
        CreateMap<FileEntity, CreateFileCommand>().ReverseMap();
        CreateMap<FileEntity, UpdateFileCommand>().ReverseMap();
        CreateMap<FileEntity, DeleteFileCommand>().ReverseMap();
    }
}