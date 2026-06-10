using ControleDietaApi.Dto;
using ControleDietaApi.Dto.ExtensionsMappings;
using ControleDietaApi.Models;
using ControleDietaApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControleDietaApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UserMetaCalController : ControllerBase
{
    private readonly INutritionService _nutritionService;

    public UserMetaCalController(INutritionService nutritionService)
    {
        _nutritionService = nutritionService;
    }
    
    /// <summary>
    /// Cria uma Meta calorica para o Usuario baseado nas perguntas
    /// </summary>
    /// <param name="userDto"></param>
    /// <returns>Os dados do usuario e a meta calorica</returns>
    /// <remarks>Retorna 200 ok com a meta calorica calculada</remarks>
    [HttpPost("calcular-meta-diaria")]
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