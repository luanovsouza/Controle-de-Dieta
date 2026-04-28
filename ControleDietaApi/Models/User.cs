using ControleDietaApi.Enum;

namespace ControleDietaApi.Models;


public class User
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public int Idade { get; set; }
    public string? Sexo { get; set; }
    public NivelAtividade AtividadeFisica { get; set; }
    public double Peso { get; set; }
    public double Altura { get; set; }
    public ObjetivoDieta Meta { get; set; } //A meta que ela vai querer pro corpo

    public double MetaDiaria { get; set; } //Meta diaria do q ela vai poder comer em questao de caloria

    public double ObterFator()
    {
        return AtividadeFisica switch
        {
            NivelAtividade.Sedentario => 1.2,
            NivelAtividade.Leve => 1.375,
            NivelAtividade.Moderado => 1.55,
            NivelAtividade.Intenso => 1.725,
            _ => 1.2
        };
    }
}
