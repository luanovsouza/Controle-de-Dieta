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
    public async Task<IActionResult> CalcularMetaDiaria([FromBody] User user)
    {
        var meta = _nutritionService.CalcularMetaDiaria(user);

        user.MetaDiaria = meta;

        return Ok(new
        {
            Mensagem = $"Ola {user.Nome}, a sua meta da sua dieta foi calculada!",
            Meta = meta,
            DadosFisicos = user
        });
    }
}
