using GestFinancas_Api.Data;
using GestFinancas_Api.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using GestFinancas_Api.Helper;
using System.Text;
using GestFinancas_Api.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using GestFinancas_Api.Dtos;
using GestFinancas_Api.Identity;
using GestFinancas_Api.Data;




namespace GestFinancas.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class UsuarioController : ControllerBase
  {
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly EnviarEmail _enviarEmail;
    private readonly IAuthenticate _authenticate;

    public UsuarioController(IUsuarioRepository usuarioRepository, IConfiguration configuration, IAuthenticate authenticate)
    {
      _usuarioRepository = usuarioRepository;
      _authenticate = authenticate;
      _enviarEmail = new EnviarEmail(configuration, _usuarioRepository);


    }

    // Método para obter todos os usuários
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> ObterTodosUsuarios()
    {
      var usuario = await _usuarioRepository.ObterTodosUsuariosAsync();

      if (usuario.Count == 0)
      {
        return NotFound(new { message = "Nenhum usuário encontrado." });
      }

      return Ok(usuario);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto login)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      var autenticado = await _authenticate.AuthenticateAscync(login.Email, login.Senha);
      if (!autenticado)
        return Unauthorized(new { message = "Email ou senha inválidos." });

      // Buscar o usuário completo após autenticação
      var usuario = await _usuarioRepository.BuscarUsuarioPorEmail(login.Email);
      if (usuario == null)
        return NotFound(new { message = "Usuário não encontrado." });

      var token = _authenticate.GenerateToken(usuario.Id, usuario.Email);
      var userToken = new UserToken { Token = token };

      return Ok(new { message = "Login realizado com sucesso", data = userToken });
    }



    [HttpPost("cadastrar-usuario")]
    public async Task<IActionResult> AddUsuario([FromBody] Usuario usuario)
    {
      if (usuario == null || !usuario.IsValid())
      {
        return BadRequest(new { message = "Dados inválidos." });
      }

      var emailExiste = await _usuarioRepository.BuscarUsuarioPorEmail(usuario.Email);
      if (emailExiste != null)
      {
        return BadRequest(new { message = "Este email já está cadastrado" });
      }

      // Aqui criamos o hash e salt da senha
      using var hmac = new System.Security.Cryptography.HMACSHA512();

      usuario.SenhaSalt = Convert.ToBase64String(hmac.Key);
      usuario.SenhaHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(usuario.Senha)));

      // Evita salvar a senha original no banco
      usuario.Senha = null;

      var usuarioId = await _usuarioRepository.AddUsuarioAsync(usuario);

      if (usuarioId == null)
      {
        return BadRequest(new { message = "Erro ao cadastrar usuário." });
      }

      var token = _authenticate.GenerateToken(usuario.Id, usuario.Email);
      var userToken = new UserToken { Token = token };

      return Ok(new { message = "Usuário cadastrado com sucesso", data = userToken });
    }



    // Método para atualizar um usuário
    // [Authorize]
    [HttpPut]
    public async Task<IActionResult> AtualizarUsuario([FromBody] Usuario usuario)
    {
      if (usuario == null || !usuario.IsValid())
      {
        return BadRequest(new { message = "Usuário inválido." });
      }

      var usuarioId = await _usuarioRepository.AtualizarUsuarioAsync(usuario);

      if (usuarioId == 0)
      {
        return BadRequest(new { message = "Erro ao atualizar usuário." });
      }

      return Ok(new { message = "Usuário atualizado com sucesso", data = usuarioId });
    }

    [HttpPost("reset-senha")]
    public async Task<IActionResult> ResetarSenha([FromBody] RedefinirSenhaDto redefinir)
    {
      if (string.IsNullOrEmpty(redefinir.Email) || string.IsNullOrEmpty(redefinir.NovaSenha))
        return BadRequest(new { message = "Email e nova senha são obrigatórios." });

      var usuario = await _usuarioRepository.BuscarUsuarioPorEmail(redefinir.Email);
      if (usuario == null)
        return NotFound(new { message = "Usuário não encontrado." });

      // Gerar novo hash + salt
      using var hmac = new System.Security.Cryptography.HMACSHA512();
      usuario.SenhaSalt = Convert.ToBase64String(hmac.Key);
      usuario.SenhaHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(redefinir.NovaSenha)));

      // Atualizar no banco
      var atualizado = await _usuarioRepository.AtualizarUsuarioAsync(usuario);

      if (atualizado == 0)
        return StatusCode(500, new { message = "Erro ao redefinir senha." });

      return Ok(new { message = "Senha redefinida com sucesso." });
    }


    private string GerarToken(Usuario usuario)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes("veryverycomplexkey1234567890");
      var identity = new ClaimsIdentity(new Claim[]
      {
       new Claim("role", "user"),
       new Claim(ClaimTypes.Name, $"{usuario.Nome}"),

      });

      var credencials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = identity,
        Expires = DateTime.UtcNow.AddDays(1),
        SigningCredentials = credencials
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      return tokenHandler.WriteToken(token);
    }





  }
}