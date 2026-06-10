using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ControleDietaApi.Enum;

namespace ControleDietaApi.Models;


public class User
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Campo obrigatorio!")]
    [StringLength(80)]
    public string? Nome { get; set; }

    [Required(ErrorMessage = "Campo obrigatorio!")]
    [Range(0, 100, ErrorMessage = "Idade entre 0 e 100!")]
    public int Idade { get; set; }

    [Required(ErrorMessage = "Campo obrigatorio!")]
    [StringLength(30)]
    public string? Sexo { get; set; }

    [Required(ErrorMessage = "Campo obrigatorio!, Sedentario,\n    Leve,\n    Moderado,\n    Intenso")]
    [StringLength(30)]
    public NivelAtividade AtividadeFisica { get; set; }

    [Required(ErrorMessage = "Campo obrigatorio!")]
    [Column(TypeName = "double(5,2)")]
    public double Peso { get; set; }

    [Required(ErrorMessage = "Campo obrigatorio!")]
    [Column(TypeName = "numeric(5,2)")]
    public double Altura { get; set; }

    [Required(ErrorMessage = "Campo obrigatorio!")]
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
