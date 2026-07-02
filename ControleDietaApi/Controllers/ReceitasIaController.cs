using System.Security.Claims;
using ControleDietaApi.Models;
using ControleDietaApi.Repositories.Interfaces;
using ControleDietaApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleDietaApi.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ReceitasIaController : ControllerBase
{
    private readonly INutritionService _nutritionService;
    private readonly IRepository<User>  _userRepository;

    public ReceitasIaController(INutritionService nutritionService, IRepository<User> userRepository)
    {
        _nutritionService = nutritionService;
        _userRepository = userRepository;
    }
    
    [HttpPost("gerar-receita")]
    //[Authorize(Roles = "User")]
    public async Task<ActionResult>? GerarReceita()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        
        //Busca o usuario no banco usando o Id do Token
        var user = await _userRepository.GetByIdAsync(u => u.Id == int.Parse(userId)); //Id do Usuario nao do guid

        if (user == null)
            return NotFound("Nenhum usuario encontrado!");

        var meta = user.MetaDiaria;
        var id = user.Id;

        var receitaGerada = await _nutritionService.GerarReceita(meta, id);
        
        return Ok(receitaGerada);
    }
}