using AutoMapper;
using ControleDietaApi.Models;

namespace ControleDietaApi.Dto.Mapping;

public class UserDtoMapping : Profile
{
    public UserDtoMapping()
    {
        CreateMap<User, UserDto>().ReverseMap();
    }
}
