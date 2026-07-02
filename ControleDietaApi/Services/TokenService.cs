using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ControleDietaApi.Dto;
using ControleDietaApi.Models;
using ControleDietaApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace ControleDietaApi.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<UserToken> _userManager;
    private readonly IUserRepository _userRepository;

    public TokenService(IConfiguration configuration, UserManager<UserToken> userManager, IUserRepository userRepository)
    {
        _configuration = configuration;
        _userManager = userManager;
        _userRepository = userRepository;
    }

    public async Task<UserTokenDto> GerarToken(UserToken userToken)
    {
        try
        {
            var secrectKey = _configuration["JWT:SecretKey"] ?? throw new Exception("SecretKey not found");

            var roles = await _userManager.GetRolesAsync(userToken);
            
            //busca o Usuario cujo UserTokenId é igual ao GUID do Identity
            var user = await _userRepository.GetByIdAsync(u => u.UserTokenId == userToken.Id);
            
            //Criando as listas de Claim
            var claims = new List<Claim>
            {
                //Coloca o int Id do banco no token
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),// Ex: int: "5"
                new Claim(ClaimTypes.Name, userToken.UserName!),
                new Claim(ClaimTypes.Email, userToken.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            //Criando as roles, e colocando nas claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var chavePrivada = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secrectKey!));
            var credenciaisToken = new SigningCredentials(chavePrivada, SecurityAlgorithms.HmacSha256);
            var expiracao = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["JWT:ExpireInMinutes"]));
            
            
            //Criando o Token
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: expiracao,
                claims: claims,
                signingCredentials: credenciaisToken
            );

            var refreshToken = GerarRefreshToken();
            
            userToken.RefreshTokenExpiration = expiracao;
            userToken.RefreshToken = refreshToken;

            return new UserTokenDto()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiracao = expiracao,
                Autenticado = true
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public string GerarRefreshToken()
    {
        var bytes = new byte[128];
        
        using var bytesAleatorios = RandomNumberGenerator.Create();
        
        bytesAleatorios.GetBytes(bytes);
        var token = Convert.ToBase64String(bytes);
        
        return token;
    }

    public ClaimsPrincipal? GerarClaimsPrincipal(string token)
    {
        throw new NotImplementedException();
    }
}