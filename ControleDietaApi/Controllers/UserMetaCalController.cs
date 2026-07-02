using System.Security.Claims;
using ControleDietaApi.Dto;
using ControleDietaApi.Dto.ExtensionsMappings;
using ControleDietaApi.Models;
using ControleDietaApi.Repositories.Interfaces;
using ControleDietaApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleDietaApi.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "User")]
public class UserMetaCalController : ControllerBase
{
    private readonly INutritionService _nutritionService;
    private readonly IUnitOfWork _ofWork;

    public UserMetaCalController(INutritionService nutritionService, IUnitOfWork ofWork)
    {
        _nutritionService = nutritionService;
        _ofWork = ofWork;
    }
    
    /// <summary>
    /// Cria uma Meta calorica para o Usuario baseado nas perguntas
    /// </summary>
    /// <param name="userDto"></param>
    /// <returns>Os dados do usuario e a meta calorica</returns>
    /// <remarks>Retorna 200 ok com a meta calorica calculada</remarks>
    /// <remarks>
    /// Meta
    /// Emagrecer,
    /// ManterPeso,
    /// GanharMassa
    /// </remarks>
    /// <remarks>
    /// Atividade Fisica
    /// Sedentario, Leve, Moderado, Intenso
    /// </remarks>
    [HttpPost("calcular-meta-diaria")]
    public async Task<IActionResult> CalcularMetaDiaria([FromBody] UserDto userDto)
    {
        if (userDto == null)
        {
            return BadRequest("Os dados do usuário não foram enviados corretamente.");
        }
        
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            return Unauthorized();
        
        
        var newUser = userDto.ToUser();
        //1 - Calcula a meta primeiro
        var meta = _nutritionService.CalcularMetaDiaria(newUser);
        newUser.MetaDiaria = meta;
        newUser.UserTokenId = userId;
        
        //Depois Salva no banco
        await _ofWork.Users.CreateAsync(newUser);
        await _ofWork.Commit();
        
        return Ok(new
        {
            Mensagem = $"Ola {newUser.Nome}, a sua meta da sua dieta foi calculada!",
            Meta = meta,
            DadosFisicos = newUser
        });
    }
}