using AutoMapper;
using Linkerly.Domain.Application.Models;

namespace Linkerly.Domain.Application.Mappings;

public class UserEntityMappingConfiguration : Profile
{
    public UserEntityMappingConfiguration()
    {
        CreateMap<UserEntity, UserEntity>().ForMember(user => user.RootFolder, options => options.Ignore())
            .ForMember(user => user.UserCreatedBy, options => options.Ignore())
            .ForMember(user => user.UserUpdatedBy, options => options.Ignore());
    }
}
