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


namespace GestFinancas_Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class EmailController : ControllerBase
  {
    private readonly EnviarEmail _enviarEmail;

    public EmailController(IConfiguration configuration)
    {
      _enviarEmail = new EnviarEmail(configuration);
    }

    /// <summary>
    /// Envia um e-mail de recuperação de senha.
    /// </summary>
    [HttpPost("recuperar-senha")]
    public IActionResult RecuperarSenha([FromBody] RecuperarSenhaDTO recuperarSenhaDTO)
    {
      if (recuperarSenhaDTO == null || string.IsNullOrEmpty(recuperarSenhaDTO.Email))
        return BadRequest("O email é obrigatório!");

      _enviarEmail.EnviarEmailRecuperacaoSenha(recuperarSenhaDTO.Email);
      return Ok("Email enviado com sucesso!");
    }
  }
}
