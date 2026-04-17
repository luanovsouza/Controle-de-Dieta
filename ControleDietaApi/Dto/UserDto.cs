namespace ControleDietaApi.Dto;

public class UserDto
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public int Idade { get; set; }
    public double Peso { get; set; }
    public double Altura { get; set; }
    public string? Meta { get; set; }
}
