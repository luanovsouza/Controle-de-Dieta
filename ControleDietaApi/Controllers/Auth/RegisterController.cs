using ControleDietaApi.Dto;
using ControleDietaApi.Models;
using ControleDietaApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ControleDietaApi.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class RegisterController : ControllerBase
{
    private readonly UserManager<UserToken> _userManager;
    private readonly SignInManager<UserToken> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly RoleManager<IdentityRole> _roleManager;

    public RegisterController(UserManager<UserToken> userManager, SignInManager<UserToken> signInManager, ITokenService tokenService, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _roleManager = roleManager;
    }

    [HttpPost("/user/Register")]
    public async Task<ActionResult> Register([FromBody] LoginDto loginDto)
    {
        var userObject = new UserToken { UserName = loginDto.UserName, Email = loginDto.Email };
        
        //Criando o usuario no banco
        var newUser = await _userManager.CreateAsync(userObject, loginDto.Password!);

        if (!newUser.Succeeded)
        {
            var erros = newUser.Errors.Select(e => e.Description);
            return BadRequest($"Ocorreu um erro ao registrar -> {string.Join(", ", erros)}");
        }

        return Ok(new
        {
            Mensagem = "Usuario Registrado com sucesso!",
        });
    }
}