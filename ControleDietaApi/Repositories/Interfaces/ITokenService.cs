using System.Security.Claims;
using ControleDietaApi.Dto;
using ControleDietaApi.Models;

namespace ControleDietaApi.Repositories.Interfaces;

public interface ITokenService
{
    public Task<UserTokenDto> GerarToken(UserToken user);
    public string GerarRefreshToken();
    public ClaimsPrincipal? GerarClaimsPrincipal(string token);
}