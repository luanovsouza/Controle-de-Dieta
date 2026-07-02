using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ControleDietaApi.Enum;

namespace ControleDietaApi.Models;


public class User
{
    [Key]
    [JsonIgnore]
    public int Id { get; set; }
    
    [JsonIgnore]
    public string UserTokenId { get; set; } = null!;
    
    [StringLength(80)]
    public string? Nome { get; set; }
    
    [Range(0, 100, ErrorMessage = "Idade entre 0 e 100!")]
    public int Idade { get; set; }
    
    [StringLength(30)]
    public string? Sexo { get; set; }
    
    [StringLength(30)]
    public NivelAtividade AtividadeFisica { get; set; }
    
    [Column(TypeName = "numeric(5,2)")]
    public double Peso { get; set; }
    
    [Column(TypeName = "numeric(5,2)")]
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
    [JsonIgnore]
    public ICollection<MeatGoal> MeatGoals { get; set; } = new List<MeatGoal>();
}
