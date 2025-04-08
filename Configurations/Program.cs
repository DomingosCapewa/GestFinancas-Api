using GestFinancas_Api.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using GestFinancas_Api.Models;
using GestFinancas_Api.Helper;
using GestFinancas_Api.Identity;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Adiciona acesso ao arquivo appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Registra o DbContext para uso com o Entity Framework
builder.Services.AddDbContext<GestFinancasContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registra o AppDbContext para uso com o Entity Framework
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Injeção de dependência
builder.Services.AddScoped<EnviarEmail>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IAuthenticate, Authenticate>();

// Autenticação JWT
builder.Services.AddAuthentication(options =>
{
  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
  options.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = builder.Configuration["Jwt:Issuer"],
    ValidAudience = builder.Configuration["Jwt:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])),
    ClockSkew = TimeSpan.Zero
  };
});

// CORS
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowLocalhost4200", policy =>
  {
    policy.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader();
  });
});

// Versionamento da API (opcional)
builder.Services.AddApiVersioning(options =>
{
  options.AssumeDefaultVersionWhenUnspecified = true;
  options.DefaultApiVersion = new ApiVersion(1, 0);
  options.ReportApiVersions = true;
});

// Controladores
builder.Services.AddControllers();

// Swagger
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
      Email = "equipe.gest.financas@gmail.com"
    }
  });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();

  // Swagger disponível apenas no desenvolvimento
  app.UseSwagger();
  app.UseSwaggerUI(c =>
  {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "GestFinancas API v1");
    c.RoutePrefix = string.Empty;
  });
}

// Middleware de CORS
app.UseCors("AllowLocalhost4200");

// Middleware de autenticação e autorização
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
