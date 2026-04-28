using ControleDietaApi.Enum;

namespace ControleDietaApi.Dto;

public class UserDto
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public int Idade { get; set; }
    public string? Sexo { get; set; }
    public NivelAtividade AtividadeFisica { get; set; }
    public ObjetivoDieta Meta { get; set; }
    public double Peso { get; set; }
    public double Altura { get; set; }
}
