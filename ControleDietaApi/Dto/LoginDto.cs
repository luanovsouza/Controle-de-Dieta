using System.ComponentModel.DataAnnotations;

namespace ControleDietaApi.Dto;

public class LoginDto
{
    [Required]
    [StringLength (30, MinimumLength = 3)]
    public string? UserName { get; set; }
    
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
}