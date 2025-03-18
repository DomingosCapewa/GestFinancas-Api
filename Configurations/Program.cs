using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using GestFinancas.Data;

var builder = WebApplication.CreateBuilder(args);

// Registrar o repositório de usuários
builder.Services.AddScoped<UsuarioRepository>(sp =>
    new UsuarioRepository(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();  // Habilitar os endpoints da API 
builder.Services.AddSwaggerGen();

// Configurar CORS
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAllOrigins", builder =>
      builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

// Configuração de CORS
app.UseCors("AllowAllOrigins");  // Habilitar CORS para todas as origens

// Habilitar o Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
