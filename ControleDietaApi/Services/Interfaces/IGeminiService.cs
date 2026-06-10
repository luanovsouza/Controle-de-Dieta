namespace ControleDietaApi.Services.Interfaces;

public interface IGeminiService
{
    //Método para analisar a comida enviada
    Task<string> AnalisarRefeicaoAsync(string descricaoRefeicao);
}