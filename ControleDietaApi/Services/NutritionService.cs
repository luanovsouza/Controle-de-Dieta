

using ControleDietaApi.Enum;
using ControleDietaApi.Models;
using ControleDietaApi.Services.Interfaces;

namespace ControleDietaApi.Services;

public class NutritionService : INutritionService
{
    public double CalcularMetaDiaria(User user) //Serve para definir o orçamento de energia durante 24hrs
    {
        double tmb;
        //Ou seja, quanto ela pode comer no dia, por exemplo 2.000 kcal
        if (user.Sexo.ToLower() == "homem" || user.Sexo.ToLower() == "masculino")
        {
            tmb = (10.00 * user.Peso) + (6.25 * user.Altura) -
              (5.0 * user.Idade) + 5.0;
        }

        else if (user.Sexo.ToLower() == "mulher" || user.Sexo.ToLower() == "feminino")
        {
            tmb = (10.00 * user.Peso) + (6.25 * user.Altura) -
              (5.0 * user.Idade) - 161.0;
        }
        else
        {
            throw new ArgumentException("Sexo invalido para calculo!");
        }


        double gastoTotal = tmb * user.ObterFator();

        return user.Meta switch
        {
            ObjetivoDieta.Emagrecer => gastoTotal - 500,
            ObjetivoDieta.GanharMassa => gastoTotal + 300,
            _ => gastoTotal
        };
    }

    public Task<MeatGoal> ProcessarRefeicao(string descricao, int userId)
    {
        throw new NotImplementedException();
    }
}
