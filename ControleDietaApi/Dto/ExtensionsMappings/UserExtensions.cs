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
            Sexo = user.Sexo,
            AtividadeFisica = user.AtividadeFisica,
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
            Sexo = userDto.Sexo,
            AtividadeFisica = userDto.AtividadeFisica,
            Altura = userDto.Altura,
            Peso = userDto.Peso,
            Meta = userDto.Meta
        };
    }
}
