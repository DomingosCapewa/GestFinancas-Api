using Microsoft.AspNetCore.Mvc;
using GestFinancas.Data;
using GestFinancas.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

using System.Net.Mail;
using System.Text;



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

    // Método para obter todos os usuários
    // [Authorize]
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

    // Método de login de usuário
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Usuario usuario)
    {
      if (usuario == null || string.IsNullOrEmpty(usuario.Email) || string.IsNullOrEmpty(usuario.Senha))
      {
        return BadRequest(new { message = "Email e senha são obrigatórios." });
      }

      var usuarioEncontrado = await _usuarioRepository.ObterUsuarioPorEmailSenhaAsync(usuario.Email, usuario.Senha);

      if (usuarioEncontrado == null)
      {
        return NotFound(new { message = "Usuário não encontrado ou senha incorreta." });
      }

      
      return Ok(new { message = "Login efetuado com sucesso", data = usuarioEncontrado });
    }

    // Método para redefinir a senha do usuário
    [HttpPost("reset-senha")]
    public async Task<IActionResult> ResetarSenha([FromBody] Usuario usuario)
    {
      if (usuario == null || string.IsNullOrEmpty(usuario.Email) || string.IsNullOrEmpty(usuario.Senha))
      {
        return BadRequest(new { message = "Email e senha são obrigatórios." });
      }

      var usuarioEncontrado = await _usuarioRepository.ResetarSenhaUsuario(usuario.Email, usuario.Senha);

      if (usuarioEncontrado == null)
      {
        return NotFound(new { message = "Usuário não encontrado ou senha incorreta." });
      }

      return Ok(new { message = "Senha alterada com sucesso", data = usuarioEncontrado });
    }
    // Método para adicionar um novo usuário
    // [Authorize]
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



    // // Método para deletar um usuário
    // // [Authorize]
    // [HttpDelete("{id}")]
    // public async Task<IActionResult> DeleteUsuario(int id)
    // {
    //   var usuarioId = await _usuarioRepository.DeleteUsuarioAsync(id);

    //   if (usuarioId == 0)
    //   {
    //     return BadRequest(new { message = "Erro ao deletar usuário." });
    //   }

    //   return Ok(new { message = "Usuário deletado com sucesso", data = usuarioId });
    // }
    // [HttpDelete("{id}")]
    // public async Task<IActionResult> resetarSenha(int email)
    // {
    //   var existeEmail = await _usuarioRepository.getEmail(emial);
    //   // //if validadnd se existe o email
    //   // // Sendemail _sendEmail;
    //   // // 
    //   // // 
    //   //  _sendEmail
    //   if (usuarioId == 0)
    //   {
    //     return BadRequest(new { message = "Erro ao deletar usuário." });
    //   }

    //   return Ok(new { message = "Usuário deletado com sucesso", data = usuarioId });
    // }
  }
}