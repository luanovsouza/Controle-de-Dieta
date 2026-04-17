namespace ControleDietaApi.Models;


public class MeatGoal //Vai ser oq ela comeu no dia, como se fosse um Log 
{
    public int Id { get; set; }
    public string? Description { get; set; } /// Ex: "Arroz, feijão e frango"


    // Dados que a IA vai extrair para nós
    public double Calories { get; set; }
    public double Protein { get; set; }
    public double Carbs { get; set; }
    public double Fat { get; set; }

    public DateTime ConsumedAt { get; set; } //Quando foi consumido 

    //Relacionamento
    public int UserId { get; set; }
    public User? User { get; set; }
}
