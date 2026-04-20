using ControleDietaApi.Models;

namespace ControleDietaApi.Dto.ExtensionsMappings;

public static class UserExtensions
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Nome = user.Nome,
            Idade = user.Idade,
            Altura = user.Altura,
            Peso = user.Peso,
            Meta = user.Meta
        };
    }


    public static User ToUser(this UserDto userDto)
    {
        return new User
        {
            Id = userDto.Id,
            Nome = userDto.Nome,
            Idade = userDto.Idade,
            Altura = userDto.Altura,
            Peso = userDto.Peso,
            Meta = userDto.Meta
        };
    }
}
