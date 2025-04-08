using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using GestFinancas_Api.Models;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;





namespace GestFinancas.Services
{
  public class TokenService
  {
    // Este método gera o token JWT
    public static dynamic GenerateToken(Usuario usuario)
    {
      // Definir a chave secreta (geralmente vem de uma variável de ambiente ou configuração)
      var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET_KEY")); // Substitua por sua chave secreta

      // Criar a descrição do token (SecurityTokenDescriptor)
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new Claim[]
          {
                    new Claim("id", usuario.Id.ToString()),
                    new Claim(ClaimTypes.Name, usuario.Nome),
                    new Claim(ClaimTypes.Email, usuario.Email)
          }),
        Expires = DateTime.UtcNow.AddHours(3), // Definindo tempo de expiração do token
        SigningCredentials = new SigningCredentials(
              new SymmetricSecurityKey(key), // Usando a chave secreta para assinatura
              SecurityAlgorithms.HmacSha256Signature // Algoritmo de assinatura
          )
      };

      // Criar o token JWT usando o descriptor
      var tokenHandler = new JwtSecurityTokenHandler();
      var token = tokenHandler.CreateToken(tokenDescriptor); // Gerar o token
      var tokenString = tokenHandler.WriteToken(token); // Converter para string

      // Retornar o token com as informações do usuário
      return new
      {
        usuario = usuario,
        token = tokenString
      };
    }
  }
}



public class TokenService
{
  public string GerarTokenRecuperacao()
  {
    // Gerar um token aleatório
    var token = Guid.NewGuid().ToString();
    return token;
  }
}
