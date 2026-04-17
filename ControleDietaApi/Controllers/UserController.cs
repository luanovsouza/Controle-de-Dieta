using AutoMapper;
using ControleDietaApi.Dto;
using ControleDietaApi.Models;
using ControleDietaApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControleDietaApi.Controllers;


[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly INutritionService _nutritionService;
    private readonly IMapper _mapper;


    public UserController(INutritionService nutritionService, IMapper mapper)
    {
        _nutritionService = nutritionService;
        _mapper = mapper;
    }


    [HttpPost("api/calcular-meta-diaria")]
    public async Task<IActionResult> CalcularMetaDiaria([FromBody] UserDto userDto)
    {
        var newUserDto = _mapper.Map<User>(userDto);

        var meta = _nutritionService.CalcularMetaDiaria(newUserDto);

        newUserDto.MetaDiaria = meta;

        var user = _mapper.Map<UserDto>(User);



        return Ok(new
        {
            Mensagem = $"Ola {user.Nome}, a sua meta da sua dieta foi calculada!",
            Meta = meta,
            DadosFisicos = user
        });
    }
}
