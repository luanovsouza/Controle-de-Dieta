using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using ControleDietaApi.Context;
using ControleDietaApi.Models;
using ControleDietaApi.Repositories;
using ControleDietaApi.Repositories.Interfaces;
using ControleDietaApi.Services;
using ControleDietaApi.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSwaggerGen();
//Para poder escrever os enums como string no JSON, ao invés de números inteiros
builder.Services.AddControllers()
  .AddJsonOptions(opt => opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();


var secretKey = builder.Configuration["JWT:SecretKey"] ?? throw new InvalidOperationException("SecretKey não configurada");


builder.Services
    .AddIdentityCore<UserToken>()
    .AddRoles<IdentityRole>()
    .AddSignInManager()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


//Configurações de Autenticação Jwt
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});


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
builder.Services.AddScoped<INutritionService, NutritionService>();
builder.Services.AddScoped<ITokenService, TokenService>();

//Repositorios
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMealsUserRepository, MealUserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();


// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     
// }

app.UseSwagger();
app.UseSwaggerUI(opt => opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Api Dieta"));

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors(OrigemComAcessoPermitido);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
