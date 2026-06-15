using System.Reflection;
using System.Text.Json.Serialization;
using ControleDietaApi.Context;
using ControleDietaApi.Repositories;
using ControleDietaApi.Repositories.Interfaces;
using ControleDietaApi.Services;
using ControleDietaApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSwaggerGen();
//Para poder escrever os enums como string no JSON, ao invés de números inteiros
builder.Services.AddControllers()
  .AddJsonOptions(opt => opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();

//Configurações de Serviços e Interfaces
builder.Services.AddScoped<INutritionService, NutritionService>();

var OrigemComAcessoPermitido = "_origemComAcessoPermitido";

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseNpgsql(connectionString);
});

//Habilitando Cors para permitir o acesso do Site ApiRequest
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(OrigemComAcessoPermitido,
        policy =>
        {
            policy.WithOrigins("https://apirequest.io").AllowAnyMethod(). //Qualquer método Http
            AllowAnyHeader().//Qualquer Header
            AllowCredentials();// Permitir o envio de cookies e credenciais de autenticação
        });
});


// builder.Services.AddSwaggerGen(c =>
// {
//     var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//     var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
//     c.IncludeXmlComments(xmlPath);
// });

//Serviços
builder.Services.AddScoped<INutritionService, NutritionService>();

//Repositorios
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMealsUserRepository, MealUserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opt => opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Api Dieta"));
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors(OrigemComAcessoPermitido);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
