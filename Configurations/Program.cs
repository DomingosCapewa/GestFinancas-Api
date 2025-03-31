using GestFinancas.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using GestFinancas_Api.Models;


var builder = WebApplication.CreateBuilder(args);

// Adiciona acesso ao arquivo appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Registra o DbContext para uso com o Entity Framework
builder.Services.AddDbContext<GestFinancasContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adiciona o serviço do repositório de usuário
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// Adiciona controladores e endpoints
builder.Services.AddControllers();

// Configura o Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
  options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
  {
    Title = "GestFinancas API",
    Version = "v1",
    Description = "API para gerenciamento de usuários e finanças",
    Contact = new Microsoft.OpenApi.Models.OpenApiContact
    {
      Name = "Suporte",
      Email = "suporte@exemplo.com"
    }
  });
});

var app = builder.Build();

// Configura o pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();

  app.UseSwagger();
  app.UseSwaggerUI(c =>
  {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "GestFinancas API v1");
    c.RoutePrefix = string.Empty;
  });
}

// Configura autenticação e autorização
app.UseRouting();
app.UseAuthentication();  
app.UseAuthorization();

// Mapeia os controladores
app.MapControllers();

app.Run();

