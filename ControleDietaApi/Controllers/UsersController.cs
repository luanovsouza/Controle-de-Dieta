using System.Security.Claims;
using ControleDietaApi.Models;
using ControleDietaApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleDietaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly IRepository<User> _userRepository;
    private readonly IUnitOfWork _uof;

    public UsersController(IRepository<User> userRepository, IUnitOfWork uof)
    {
        _userRepository = userRepository;
        _uof = uof;
    }


    [HttpGet]
    public ActionResult<IEnumerable<User>> Get()
    {
        
        var users = _userRepository.GetAll();

        if (users == null)
            return BadRequest("Nao foi possivel achar");

        return Ok(users);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult<User>> Get(int id)
    {
        var userById = await _userRepository.GetByIdAsync(userid => userid.Id == id);

        if (userById == null)
            return NotFound("Nenhum usuario foi Encontrado!");
        
        return Ok(userById);
    }

    [HttpDelete]
    [Authorize(Roles = "AdminOnly")]
    public async Task<ActionResult<User>> DeleteAsync(int id)
    {
        //Verifica se a pessoa ta logada e se nao tem autorização
        var userIdLogado = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if(userIdLogado == null)
            return Unauthorized("Usuario Nao Autorizado!");

        if (userIdLogado != id.ToString())
            return Forbid(); //está logado, mas tentando deletar conta de outra pessoa, entao so pode deletar a propia conta
        
        
        var userDeleted = await _userRepository.GetByIdAsync(userid => userid.Id == id);

        if (userDeleted is null)
            return NotFound("Nao foi possivel achar esse usuario!");
        
        await _userRepository.Delete(userDeleted);
        
        return Ok(userDeleted);
    }
}