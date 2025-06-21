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
   
    public static dynamic GenerateToken(Usuario usuario)
    {
     
      var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET_KEY")); 

      
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
              new SymmetricSecurityKey(key),
              SecurityAlgorithms.HmacSha256Signature
          )
      };

      // Criar o token JWT usando o descriptor
      var tokenHandler = new JwtSecurityTokenHandler();
      var token = tokenHandler.CreateToken(tokenDescriptor); 
      var tokenString = tokenHandler.WriteToken(token); 

      
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
   
    var token = Guid.NewGuid().ToString();
    return token;
  }
}
