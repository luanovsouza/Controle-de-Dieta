using ControleDietaApi.Dto;
using ControleDietaApi.Models;
using ControleDietaApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ControleDietaApi.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly UserManager<UserToken> _userManager;
    private readonly SignInManager<UserToken> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly RoleManager<IdentityRole> _roleManager;

    public LoginController(UserManager<UserToken> userManager, SignInManager<UserToken> signInManager, ITokenService tokenService, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _roleManager = roleManager;
    }


    [HttpPost("/user/login")]
    public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
    {
        //Buscanjdo o usuario
        var user = await _userManager.FindByEmailAsync(loginDto.Email!);

        if (user is null)
            return BadRequest("Email Incorreto, digite novamente!");
        
        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password!, false).ConfigureAwait(false);

        if (!result.Succeeded)
        {
            return BadRequest($"Senha incorreta!");
        }

        var token = await _tokenService.GerarToken(user);

        await _userManager.UpdateAsync(user);

        return Ok(new
        {
            Mensagem = "Usuario Logado com Sucesso!",
            Token = token
        });
    }
}