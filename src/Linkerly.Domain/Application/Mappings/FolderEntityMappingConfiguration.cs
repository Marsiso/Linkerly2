using AutoMapper;
using Linkerly.Domain.Application.Models;

namespace Linkerly.Domain.Application.Mappings;

public class FolderEntityMappingConfiguration : Profile
{
    public FolderEntityMappingConfiguration()
    {
        CreateMap<FolderEntity, FolderEntity>().ForMember(user => user.User, options => options.Ignore())
            .ForMember(user => user.Parent, options => options.Ignore())
            .ForMember(user => user.Type, options => options.Ignore())
            .ForMember(user => user.Files, options => options.Ignore())
            .ForMember(user => user.Children, options => options.Ignore())
            .ForMember(user => user.UserCreatedBy, options => options.Ignore())
            .ForMember(user => user.UserUpdatedBy, options => options.Ignore());
    }
}
