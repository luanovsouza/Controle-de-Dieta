namespace ControleDietaApi.Dto;

public class ReceitaDto
{
    public string? Nome { get; set; }
    public List<string>? Ingredientes { get; set; }
    public string? ModoDePreparo { get; set; }
    public int Calorias { get; set; }
    public int Gordura { get; set; }
    public int Proteinas { get; set; }
}