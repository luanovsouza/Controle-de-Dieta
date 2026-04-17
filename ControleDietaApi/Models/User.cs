namespace ControleDietaApi.Models;


public class User
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? MyProperty { get; set; }
    public int Idade { get; set; }
    public double Peso { get; set; }
    public double Altura { get; set; }
    public string Meta { get; set; } = "Emagrecer"; //A meta que ela vai querer pro corpo

    public double MetaDiaria { get; set; } //Meta diaria do q ela vai poder comer em questao de caloria
}
