using ControleDietaApi.Dto;
using ControleDietaApi.Dto.ExtensionsMappings;
using ControleDietaApi.Enum;
using ControleDietaApi.Models;
using ControleDietaApi.Repositories.Interfaces;
using ControleDietaApi.Services.Interfaces;
using GenerativeAI;
using Microsoft.Extensions.AI;
using Newtonsoft.Json;
using OllamaSharp;

namespace ControleDietaApi.Services;

public class NutritionService : INutritionService
{
    private readonly IChatClient _clientChat;
    private List<ChatMessage> historicoChat = new();
    private readonly IRepository<MeatGoal> _repository;
    private readonly IUnitOfWork _uof;

    public NutritionService(IRepository<MeatGoal> repository, IUnitOfWork uof)
    {
        _repository = repository;
        _uof = uof;
        _clientChat = new OllamaApiClient(new Uri("http://localhost:11434"), "llama3.2");
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
        Você é uma API REST.

        Entrada:
        {descricao}

        Sua tarefa:
        Analise a seguinte refeição brasileira (se nao for, identifique (a)): '{descricao}'.
        Estime as calorias (Calories), proteínas (Protein), carboidratos (Carbs) e gorduras (Fat) da refeiçao.


        Se a quantidade não for informada:

        - assuma UMA porção média consumida no Brasil;
        - frutas → unidade média;
        - massas → prato médio;
        - arroz → porção média;
        - carnes → porção média;
        - bebidas → copo médio.

        IMPORTANTE:

        - Responda SOMENTE JSON.
        - Não escreva código.
        - Não escreva funções.
        - Não explique.
        - Não escreva texto.
        - Não use markdown.
        - Não gere pseudocódigo.
        - Não use ```.

        Use uma porção média brasileira.

        Calcule:
        Calories = Protein*4 + Carbs*4 + Fat*9

        Retorne exatamente este formato:

        {{
        ""Calories"": 0,
        ""Protein"": 0,
        ""Carbs"": 0,
        ""Fat"": 0
        }}

        JSON:";
        try
        {
            var resposta = await _clientChat.GetResponseAsync(prompt);
            var jsonTexto = resposta.Messages.Last().Text;

            Console.WriteLine($">>> Resposta Ollama: {jsonTexto}");

            jsonTexto = jsonTexto
                .Replace("```json", "")
                .Replace("```", "")
                .Replace("\u00a0", " ") // espaço não-quebrável
                .Replace("\u2003", " ") // espaço em
                .Replace("\u2002", " ") // espaço en
                .Trim(); // remove espaços e quebras de linha sobrando

            if (string.IsNullOrEmpty(jsonTexto))
                throw new ArgumentNullException("Resposta veio vazia!");
            
            var dadosExtraidos = JsonConvert.DeserializeObject<MeatGoal>(jsonTexto);

            if (dadosExtraidos == null)
            {
                throw new Exception("JSON Invalido!");
            }

            //Dados para salvar no banco
            dadosExtraidos.UserId = userId;
            dadosExtraidos.Description = descricao;
            dadosExtraidos.ConsumedAt = DateTime.UtcNow;
            
            await _repository.Create(dadosExtraidos);
            await _uof.Commit();

            //Transformando o model em Dto
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