using Microsoft.AspNetCore.Identity;

namespace ControleDietaApi.Models;

public class UserToken : IdentityUser
{
     public string? RefreshToken { get; set; }
     public DateTime? RefreshTokenExpiration { get; set; }
}