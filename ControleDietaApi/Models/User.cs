namespace ControleDietaApi.Models;


public class User
{
    public int Id { get; set; }
    public string? MyProperty { get; set; }
    public int Idade { get; set; }
    public decimal Peso { get; set; }
    public decimal Altura { get; set; }
    public string Meta { get; set; } = "Emagrecer";

    public string? MetaDiaria { get; set; } //Meta diaria do q ela vai poder comer
}
