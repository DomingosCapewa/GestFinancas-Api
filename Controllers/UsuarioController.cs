using Microsoft.AspNetCore.Mvc;
using GestFinancas.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestFinancas.Models;

namespace GestFinancas.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class UsuarioController : ControllerBase
  {
    private readonly UsuarioRepository _usuarioRepository;

    public UsuarioController(UsuarioRepository usuarioRepository)
    {
      _usuarioRepository = usuarioRepository;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UsuarioLogin usuarioLogin)
    {
      try
      {
        Console.WriteLine("Iniciando login...");
        // Verificação de campos nulos ou vazios
        if (usuarioLogin == null || string.IsNullOrEmpty(usuarioLogin.Email) || string.IsNullOrEmpty(usuarioLogin.Senha))
        {
          Console.WriteLine("Email ou senha não fornecidos.");
          return BadRequest(new { message = "Email e senha são obrigatórios." });
        }

        // Busca o usuário no banco de dados pela combinação de email e senha
        var usuarioEncontrado = await _usuarioRepository.GetUsuarioByEmailSenhaAsync(usuarioLogin.Email, usuarioLogin.Senha);

        // Caso o usuário não seja encontrado
        if (usuarioEncontrado == null)
        {
          Console.WriteLine("Usuário não encontrado ou senha incorreta.");
          return NotFound(new { message = "Usuário não encontrado ou senha incorreta." });
        }

        // Se encontrado, retorna sucesso
        Console.WriteLine("Login efetuado com sucesso.");
        return Ok(new { message = "Login efetuado com sucesso", data = usuarioEncontrado });
      }
      catch (Exception ex)
      {
        // Em caso de erro interno
        Console.WriteLine($"Erro no login: {ex.Message}");
        return StatusCode(500, new { message = "Erro interno no servidor", error = ex.Message });
      }
    }

    // Rota para criar um novo usuário
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Usuario usuario)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(new { message = "Ocorreu um erro ao criar o usuário.", errors = ModelState });
      }

      var id = await _usuarioRepository.AddUsuarioAsync(usuario);
      return CreatedAtAction(nameof(Get), new { id = id }, new { message = "Usuário criado com sucesso!", data = usuario });
    }

    // Rota para listar todos os usuários
    [HttpGet]
    public async Task<IActionResult> Get()
    {
      var usuario = await _usuarioRepository.GetAllUsuariosAsync();
      return Ok(new { message = "Usuários retornados com sucesso.", data = usuario });
    }

    // Rota para obter usuário por ID
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
      var usuario = await _usuarioRepository.GetUsuarioByIdAsync(id);
      return usuario != null ? Ok(new { message = "Usuário retornado com sucesso", data = usuario }) : NotFound("Usuário não encontrado.");
    }

    // Rota para atualizar um usuário existente
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Usuario usuario)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(new { message = "Ocorreu um erro ao atualizar o usuário.", errors = ModelState });
      }

      usuario.Id = id;
      var result = await _usuarioRepository.UpdateUsuarioAsync(usuario);
      if (result > 0)
      {
        return Ok(new { message = "Usuário atualizado com sucesso.", data = usuario });
      }
      else
      {
        return NotFound("Usuário não encontrado.");
      }
    }

    // Rota para excluir um usuário
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      var result = await _usuarioRepository.DeleteUsuarioAsync(id);
      return result > 0 ? Ok("Usuário deletado com sucesso.") : NotFound("Usuário não encontrado.");
    }
  }
}