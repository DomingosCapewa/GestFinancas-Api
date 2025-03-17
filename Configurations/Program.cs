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

var app = builder.Build();

// Habilitar o Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();  

app.Run();
