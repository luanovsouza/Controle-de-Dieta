using AutoMapper;
using ControleDietaApi.Dto;
using ControleDietaApi.Dto.ExtensionsMappings;
using ControleDietaApi.Models;
using ControleDietaApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControleDietaApi.Controllers;


[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly INutritionService _nutritionService;

    public UserController(INutritionService nutritionService)
    {
        _nutritionService = nutritionService;
    }

    [HttpPost("api/calcular-meta-diaria")]
    public IActionResult CalcularMetaDiaria([FromBody] UserDto userDto)
    {
        if (userDto == null)
        {
            return BadRequest("Os dados do usuário não foram enviados corretamente.");
        }

        // Verificação de segurança 2: O serviço foi injetado?
        if (_nutritionService == null)
        {
            return StatusCode(500, "O serviço de nutrição não foi inicializado.");
        }

        var newUser = userDto.ToUser();

        var meta = _nutritionService.CalcularMetaDiaria(newUser);

        newUser.MetaDiaria = meta;

        return Ok(new
        {
            Mensagem = $"Ola {newUser.Nome}, a sua meta da sua dieta foi calculada!",
            Meta = meta,
            DadosFisicos = newUser
        });
    }
}
