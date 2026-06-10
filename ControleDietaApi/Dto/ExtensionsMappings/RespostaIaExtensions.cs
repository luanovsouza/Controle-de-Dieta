using ControleDietaApi.Models;

namespace ControleDietaApi.Dto.ExtensionsMappings;

public static class RespostaIaExtensions
{
    public static RespostaIaDto ToRespostaIaDto(this MeatGoal meatGoal)
    {
        return new RespostaIaDto
        {
            Calories = meatGoal.Calories,
            Protein = meatGoal.Protein,
            Carbs = meatGoal.Carbs,
            Fat = meatGoal.Fat
        };
    }
}