using ControleDietaApi.Dto.Mapping;
using ControleDietaApi.Services;
using ControleDietaApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

//Configurações de Serviços e Interfaces
builder.Services.AddScoped<INutritionService, NutritionService>();


//Configuarçoes de Mapeamento
builder.Services.AddAutoMapper(typeof(UserDtoMapping));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(opt => opt.SwaggerEndpoint("/openapi/v1.json", "Api Dieta"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
