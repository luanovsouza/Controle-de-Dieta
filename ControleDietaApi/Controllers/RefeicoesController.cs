using ControleDietaApi.Dto;
using ControleDietaApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleDietaApi.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "UserOnly")]
public class RefeicoesController : ControllerBase
{
    private readonly IMealsUserRepository _userMealRepository;
    private IUnitOfWork _uof;

    public RefeicoesController(IMealsUserRepository userRepository, IUnitOfWork uof)
    {
        _userMealRepository = userRepository;
        _uof = uof;
    }

    [HttpGet("{userId:int}")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetByUserId(int userId)
    {
        var refeicoes = await _userMealRepository.GetByUserIdAsync(userId);

        if(refeicoes == null)
            return NotFound("Usuario nao encontrado");
        
        return Ok(refeicoes);
    }
}