

using ControleDietaApi.Models;
using ControleDietaApi.Services.Interfaces;

namespace ControleDietaApi.Services;

public class NutritionService : INutritionService
{
    public double CalcularMetaDiaria(User user) //Serve para definir o orçamento de energia durante 24hrs
    {
        //Ou seja, quanto ela pode comer no dia, por exemplo 2.000 kcal

        double tmb = (double)(10.00 * user.Peso) + (6.25 * user.Altura) - (5.00 * user.Idade);

        var gastoTotal = tmb * 1.55;

        return user.Meta.ToLower() switch
        {
            "emagrecer" => gastoTotal - 450, //Se for emagrecer o gastoTotal diario vai ser cortado
            "ganhar" => gastoTotal + 300, //Se for ganhar peso, vai adicionar as calorias
            _ => gastoTotal //Manter o peso
        };
    }

    public Task<MeatGoal> ProcessarRefeicao(string descricao, int userId)
    {
        throw new NotImplementedException();
    }
}
