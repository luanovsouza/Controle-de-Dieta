

using ControleDietaApi.Dto;
using ControleDietaApi.Models;

namespace ControleDietaApi.Services.Interfaces;

public interface INutritionService
{
    double CalcularMetaDiaria(User user);

    Task<RespostaIaDto> ProcessarRefeicaoIa(string descricao, int userId);

    Task<ReceitaDto> GerarReceita(double metaCalorica, int userId);
}
