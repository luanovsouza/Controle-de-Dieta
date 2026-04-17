

using ControleDietaApi.Models;

namespace ControleDietaApi.Services.Interfaces;

public interface INutritionService
{
    double CalcularMetaDiaria(User user);

    Task<MeatGoal> ProcessarRefeicao(string descricao, int userId);
}
