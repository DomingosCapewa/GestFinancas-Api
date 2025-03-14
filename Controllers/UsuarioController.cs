using Microsoft.AspNetCore.Mvc;
using GestFinancas.Data;
using System.Collections.Generic;

using System.Threading.Tasks;

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

    [HttpGet]
    public async Task<IActionResult> Get()
    {
      var usuario = await _usuarioRepository.GetAllUsuariosAsync();
      return Ok(new { message = "Usuários retornados com sucesso.", data = usuario });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
      var usuario = await _usuarioRepository.GetUsuarioByIdAsync(id);
      return usuario != null ? Ok(new { message = "Usuário retornado com sucesso", data = usuario }) : NotFound("Usuário não encontrado.");
    }


    [HttpGet("{email}/{senha}")]
    public async Task<IActionResult> Get(string email, string senha)
    {
      var usuario = await _usuarioRepository.GetUsuarioByEmailSenhaAsync(email, senha);
      return usuario != null ? Ok(new { message = "Login efetuado com sucesso", data = usuario }) : NotFound("Usuário não encontrado.");
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Usuario usuario)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(new { message = "Ocorreu um erro ao criar o usuário.", errors = ModelState });
      }

      var id = await _usuarioRepository.AddUsuarioAsync(usuario);
      return CreatedAtAction(nameof(Get), new { id = id }, new { message = "Usuário criado com sucesso!", data = usuario  });
    }

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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      var result = await _usuarioRepository.DeleteUsuarioAsync(id);
      return result > 0 ? Ok("Usuário deletado com sucesso.") : NotFound("Usuário não encontrado.");
    }
  }
}
