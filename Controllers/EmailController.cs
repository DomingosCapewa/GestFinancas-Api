using Microsoft.AspNetCore.Mvc;
using GestFinancas_Api.Models;
using GestFinancas_Api.Helper;
using System.Threading.Tasks;
using GestFinancas_Api.Data;

namespace GestFinancas_Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class EmailController : ControllerBase
  {
    private readonly EnviarEmail _enviarEmail;
    private readonly IUsuarioRepository _usuarioRepository;


    public EmailController(EnviarEmail enviarEmail, IUsuarioRepository usuarioRepository)
    {
      _enviarEmail = enviarEmail;
      _usuarioRepository = usuarioRepository;
    }

    /// <summary>

    /// </summary>
    [HttpPost("email-recuperacao-senha")]
    public async Task<IActionResult> RecuperarSenha([FromBody] Usuario usuario)
    {
      if (usuario == null || string.IsNullOrEmpty(usuario.Email))
        return BadRequest("O email é obrigatório!");


      await _enviarEmail.EnviarEmailRecuperacaoSenha(usuario.Email);
      return Ok("Email enviado com sucesso!");
    }

    [HttpPost("confirmar-cadastro")]
    public async Task<IActionResult> ConfirmarCadastro([FromBody] Usuario usuario)
    {
      if (usuario == null || string.IsNullOrEmpty(usuario.Email))
        return BadRequest("O email é obrigatório!");

      await _enviarEmail.EnviarEmailConfirmacaoCadastro(usuario.Email, usuario.Nome);
      return Ok("Email enviado com sucesso!");
    }

  }
}
