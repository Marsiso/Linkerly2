using AutoMapper;
using Linkerly.Core.Application.Users.Commands;
using Linkerly.Domain.Application.Models;

namespace Linkerly.Core.Application.Users.Mappings;

public class UserCommandMappingConfiguration : Profile
{
    public UserCommandMappingConfiguration()
    {
        CreateMap<UserEntity, CreateUserCommand>().ReverseMap();
        CreateMap<UserEntity, UpdateUserCommand>().ReverseMap();
        CreateMap<UserEntity, DeleteUserCommand>().ReverseMap();
    }
}
