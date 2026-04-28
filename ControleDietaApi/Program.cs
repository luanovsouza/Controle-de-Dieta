using System.Text.Json.Serialization;
using ControleDietaApi.Services;
using ControleDietaApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
  .AddJsonOptions(opt =>
      {
          opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
      });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

//Configurações de Serviços e Interfaces
builder.Services.AddScoped<INutritionService, NutritionService>();

var OrigemComAcessoPermitido = "_origemComAcessoPermitido";

builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: OrigemComAcessoPermitido,
        policy =>
        {
            policy.WithOrigins("https://apirequest.io").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(opt => opt.SwaggerEndpoint("/openapi/v1.json", "Api Dieta"));
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors(OrigemComAcessoPermitido);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
