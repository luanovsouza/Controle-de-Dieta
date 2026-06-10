// using ControleDietaApi.Services.Interfaces;
// using GenerativeAI;
// using Microsoft.AspNetCore.Mvc.ModelBinding;
//
// namespace ControleDietaApi.Services;
//
// public class GeminiService : IGeminiService
// {
//     private readonly GenerativeModel _model;
//
//     // O .NET injeta o IConfiguration automaticamente aqui
//     public GeminiService(IConfiguration configuration)
//     {
//         //Pegando a Chave de API
//         var apiKey = configuration["Gemini:ApiKey"];
//         
//         //Se nao achar a chave, vai lancar uma exceção
//         if (string.IsNullOrEmpty(apiKey))
//         {
//             throw new ArgumentNullException("A chave do Gemini não foi encontrada no appsettings.json");
//         }
//         
//         
//         var modelo = new GenerativeModel(apiKey, "gemini-1.5-flash");
//     }
//     
//     public async Task<string> AnalisarRefeicaoAsync(string descricaoRefeicao)
//     {
//         var promptIa = $"Atue como um nutricionista rigoroso. Analise as " +
//                        $"seguinte refeição e estime as calorias, " +
//                        $"carboidratos, proteínas e gorduras. Seja direto e retorne em " +
//                        $"tópicos: {descricaoRefeicao}";
//
//         var respostaIa = await _model.GenerateContentAsync(promptIa);
//
//         return respostaIa.Text ?? "Nao foi possivel obter as respostas da IA";
//     }
// }