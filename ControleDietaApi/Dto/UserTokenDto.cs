namespace ControleDietaApi.Dto;

public class UserTokenDto
{
    public bool Autenticado { get; set; }
    public DateTime Expiracao { get; set; }
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }

}