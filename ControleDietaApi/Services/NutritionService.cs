using System.Text.Json;
using ControleDietaApi.Dto;
using ControleDietaApi.Dto.ExtensionsMappings;
using ControleDietaApi.Enum;
using ControleDietaApi.Models;
using ControleDietaApi.Services.Interfaces;
using GenerativeAI;

namespace ControleDietaApi.Services;

public class NutritionService : INutritionService
{

    private readonly GenerativeModel _model;
    private RespostaIaDto _respostaIaDto;

    public NutritionService(IConfiguration configuration, RespostaIaDto respostaIaDto)
    {
        _respostaIaDto = respostaIaDto;
        //Pegando a Chave de API
        var apiKey = configuration["Gemini:ApiKey"];
        
        //Se nao achar a chave, vai lancar uma exceção
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new ArgumentNullException("A chave do Gemini não foi encontrada no appsettings.json");
        }
        
        var modelo = new GenerativeModel(apiKey, "gemini-1.5-flash");
    }
    
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

    public async Task<RespostaIaDto> ProcessarRefeicaoIa(string descricao, int userId)
    {
        var prompt = $@"
        Analise a seguinte refeição brasileira (se nao for, identifique (a)): '{descricao}'.
        Estime as calorias (Calories), proteínas (Protein), carboidratos (Carbs) e gorduras (Fat).
        
        Você DEVE responder ESTRITAMENTE em formato JSON válido, sem formatações de markdown (não coloque os blocos de código ```json ou ```).
        Envie apenas e exclusivamente um objeto JSON exatamente com a seguinte estrutura:
        {{
            ""Calories"": 0.0,
            ""Protein"": 0.0,
            ""Carbs"": 0.0,
            ""Fat"": 0.0
        }}";

        try
        {
            var respostaIa = _model.GenerateContentAsync(prompt);

            var jsonTexto = respostaIa.ToString();

            if (string.IsNullOrEmpty(jsonTexto))
                throw new ArgumentNullException("Os dados vieram vazios");
            
            // 3. Convertemos a String JSON da IA diretamente em um objeto C# do tipo MeatGoal
            var dadosExtraidos = JsonSerializer.Deserialize<MeatGoal>(jsonTexto, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Evita erros se as chaves virem minúsculas/maiúsculas
            });

            var dadosExtraidosDto = dadosExtraidos.ToRespostaIaDto();
            
            if (dadosExtraidosDto == null)
                throw new ArgumentNullException(nameof(dadosExtraidos));
            

            return dadosExtraidosDto;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ocorreu um erro ao processar o formato da refeição: {e.Message}");
            throw;
        }
    }
}