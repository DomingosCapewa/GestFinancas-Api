using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using GestFinancas_Api.Helper;
using GestFinancas_Api.Models;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using GestFinancas_Api.Models;
using GestFinancas.Data;


namespace GestFinancas_Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class EmailController : ControllerBase
  {
    private readonly EnviarEmail _enviarEmail;
    private readonly IUsuarioRepository _usuarioRepository;

    public EmailController(IConfiguration configuration, IUsuarioRepository usuarioRepository)
    {
      _enviarEmail = new EnviarEmail(configuration);
      _usuarioRepository = usuarioRepository;
    }

    /// <summary>
    /// Envia um e-mail de recuperação de senha.
    /// </summary>
    [HttpPost("recuperar-senha")]
    public IActionResult RecuperarSenha([FromBody] Usuario usuario)
    {
      if (usuario == null || string.IsNullOrEmpty(usuario.Email))
        return BadRequest("O email é obrigatório!");

      _enviarEmail.EnviarEmailRecuperacaoSenha(usuario.Email);
      return Ok("Email enviado com sucesso!");
    }
  }
}
