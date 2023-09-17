using AutoMapper;
using Linkerly.Domain.Application.Models;

namespace Linkerly.Domain.Application.Mappings;

public class FileEntityMappingConfiguration : Profile
{
    public FileEntityMappingConfiguration()
    {
        CreateMap<FileEntity, FileEntity>().ForMember(user => user.Folder, options => options.Ignore())
                                           .ForMember(user => user.Extension, options => options.Ignore())
                                           .ForMember(user => user.UserCreatedBy, options => options.Ignore())
                                           .ForMember(user => user.UserUpdatedBy, options => options.Ignore());
    }
}