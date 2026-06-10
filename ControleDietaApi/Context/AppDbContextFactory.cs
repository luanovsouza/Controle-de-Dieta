using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ControleDietaApi.Context;

//essa classe sabe criar um AppDbContext em tempo de design
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    /// <summary>
    /// Criando a classe de Contexto a mao
    /// </summary>
    /// <param"></param>
    /// <returns></returns>
    public AppDbContext CreateDbContext(string[] args)
    {
        //Monta um leitor de configurações manualmente — o
        //mesmo que o Program.cs faz automaticamente quando a
        //aplicação sobe. Aqui você faz na mão porque o CLI não sobe a aplicação.
        
        //SetBasePath → diz onde procurar o arquivo
        // AddJsonFile → diz qual arquivo ler
        //Build → constrói o leitor
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        
        //Configura as opções do contexto — aqui você diz: "use PostgreSQL com essa connection string".
        //É o mesmo que você faz no Program.cs com builder.Services.AddDbContext.
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(config.GetConnectionString("DefaultConnection"));

        //Cria e retorna o contexto já configurado para o CLI usar.
        return new AppDbContext(optionsBuilder.Options);
    }
}