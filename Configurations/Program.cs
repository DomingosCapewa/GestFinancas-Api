using GestFinancas.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Adiciona acesso ao arquivo appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Adiciona o serviço do repositório de usuário
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// Adiciona controladores e endpoints
builder.Services.AddControllers();

// Configura o Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(


);  // Certifique-se de adicionar isso

var app = builder.Build();

// Configura o pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();

  app.UseSwagger();
  app.UseSwaggerUI();  // Disponibiliza a interface do Swagger
}

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
