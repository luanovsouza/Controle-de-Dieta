using ControleDietaApi.Dto;
using ControleDietaApi.Repositories.Interfaces;
using ControleDietaApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleDietaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProcessFoodIaController : ControllerBase
{
    private readonly INutritionService _nutritionService;
    private readonly IUnitOfWork _ofWork;

    public ProcessFoodIaController(INutritionService nutritionService, IUnitOfWork ofWork)
    {
        _nutritionService = nutritionService;
        _ofWork = ofWork;
    }

    [HttpPost("refeicao")]
    public async Task<ActionResult> ProcesarRefeicao([FromBody] ProcessarRefeicaoDto processarRefeicaoDto)
    {
        if (processarRefeicaoDto == null)
            return BadRequest("A descrição nao pode ser vazia!");
        
        var result = await _nutritionService.ProcessarRefeicaoIa(processarRefeicaoDto.Descricao, 
            processarRefeicaoDto.UserId);
        
        return Ok(new
        {
            Message = "Aqui esta o resumo do que voce comeu",
            Resultado = result
        });
    }
}