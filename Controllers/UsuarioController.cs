using Microsoft.AspNetCore.Mvc;
using GestFinancas.Data;
using GestFinancas.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestFinancas.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class UsuarioController : ControllerBase
  {
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioController(IUsuarioRepository usuarioRepository)
    {
      _usuarioRepository = usuarioRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsuarios()
    {
      var usuarios = await _usuarioRepository.GetAllUsuariosAsync();

      if (usuarios.Count == 0)
      {
        return NotFound(new { message = "Nenhum usuário encontrado." });
      }

      return Ok(usuarios);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Usuario usuario)
    {
      if (usuario == null || string.IsNullOrEmpty(usuario.Email) || string.IsNullOrEmpty(usuario.Senha))
      {
        return BadRequest(new { message = "Email e senha são obrigatórios." });
      }

      var usuarioEncontrado = await _usuarioRepository.GetUsuarioByEmailSenhaAsync(usuario.Email, usuario.Senha);

      if (usuarioEncontrado == null)
      {
        return NotFound(new { message = "Usuário não encontrado ou senha incorreta." });
      }

      return Ok(new { message = "Login efetuado com sucesso", data = usuarioEncontrado });
    }

    [HttpPost]
    public async Task<IActionResult> AddUsuario([FromBody] Usuario usuario)
    {
      if (usuario == null || !usuario.IsValid())
      {
        return BadRequest(new { message = "Usuário inválido." });
      }

      var usuarioId = await _usuarioRepository.AddUsuarioAsync(usuario);

      if (usuarioId == 0)
      {
        return BadRequest(new { message = "Erro ao adicionar usuário." });
      }

      return Ok(new { message = "Usuário adicionado com sucesso", data = usuarioId });
    }


    [HttpPut]
    public async Task<IActionResult> UpdateUsuario([FromBody] Usuario usuario)
    {
      if (usuario == null || !usuario.IsValid())
      {
        return BadRequest(new { message = "Usuário inválido." });
      }

      var usuarioId = await _usuarioRepository.UpdateUsuarioAsync(usuario);

      if (usuarioId == 0)
      {
        return BadRequest(new { message = "Erro ao atualizar usuário." });
      }

      return Ok(new { message = "Usuário atualizado com sucesso", data = usuarioId });
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUsuario(int id)
    {
      var usuarioId = await _usuarioRepository.DeleteUsuarioAsync(id);

      if (usuarioId == 0)
      {
        return BadRequest(new { message = "Erro ao deletar usuário." });
      }

      return Ok(new { message = "Usuário deletado com sucesso", data = usuarioId });
    }

  

  }

}