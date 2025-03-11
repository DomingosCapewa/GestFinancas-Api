using System;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Usuarios.DB;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Swagger para Desenvolvimento
if (builder.Environment.IsDevelopment())
{
  builder.Services.AddSwaggerGen(c =>
  {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "User API", Description = "Gerenciamento de Usuários", Version = "v1" });
  });
}

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

// Configuração do Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
  c.SwaggerEndpoint("/swagger/v1/swagger.json", "User API V1");
});

// Endpoints de Usuário
app.MapGet("/usuarios", () => UsuarioDB.GetUsuarios());
app.MapGet("/usuarios/{email}/{senha}", (string email, string senha) => UsuarioDB.GetUsuario(email, senha));

app.MapPost("/usuarios", (Usuario usuario) =>
{
  if (usuario != null)
  {
    UsuarioDB.CriarUsuario(usuario.nomeUsuario, usuario.emailUsuario, usuario.senha);
    return Results.Created($"/usuarios/{usuario.emailUsuario}", usuario);
  }
  return Results.BadRequest("Dados inválidos.");
});

app.MapPut("/usuarios", (Usuario usuario) =>
{
  if (usuario != null)
  {
    UsuarioDB.AtualizarUsuario(usuario.nomeUsuario, usuario.emailUsuario, usuario.senha);
    return Results.NoContent(); 
  }
  return Results.BadRequest("Dados inválidos.");
});

app.MapDelete("/usuarios/{email}", (string email) =>
{
  Usuario? usuario = UsuarioDB.GetUsuario(email, "");
  if (email != null)
  {
    UsuarioDB.DeletarUsuario(email);
    return Results.NoContent(); 
  }
  return Results.BadRequest("Dados inválidos.");
});

app.Run();