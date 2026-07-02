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
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _uof;

    public RegisterController(UserManager<UserToken> userManager, SignInManager<UserToken> signInManager, ITokenService tokenService, RoleManager<IdentityRole> roleManager, IUserRepository userRepository, IUnitOfWork uof)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _roleManager = roleManager;
        _userRepository = userRepository;
        _uof = uof;
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
        
        //So para salvar o Guid no banco para o Identity lidar com o Login
        var user = new User
        {
            UserTokenId = userObject.Id // GUID do Identity
        };

        await _userRepository.CreateAsync(user);
        await _uof.Commit();
        
        return Ok(new
        {
            Mensagem = "Usuario Registrado com sucesso!",
        });
    }
}