using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using GestFinancas.Data;

var builder = WebApplication.CreateBuilder(args);

// Registrar o repositório de usuários
builder.Services.AddScoped<UsuarioRepository>(sp =>
    new UsuarioRepository(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddControllers();  // Habilitar o uso de controladores
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Habilitar o Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();  

app.Run();
