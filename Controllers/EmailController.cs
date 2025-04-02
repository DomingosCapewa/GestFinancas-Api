using Microsoft.AspNetCore.Mvc;
using GestFinancas_Api.Models;
using GestFinancas_Api.Helper;
using System.Threading.Tasks;
using GestFinancas.Data;

namespace GestFinancas_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly EnviarEmail _enviarEmail;
        private readonly IUsuarioRepository _usuarioRepository;

        // O EnviarEmail e o IUsuarioRepository agora são injetados via DI
        public EmailController(EnviarEmail enviarEmail, IUsuarioRepository usuarioRepository)
        {
            _enviarEmail = enviarEmail;
            _usuarioRepository = usuarioRepository;
        }

        /// <summary>
        /// Envia um e-mail de recuperação de senha.
        /// </summary>
        [HttpPost("recuperar-senha")]
        public async Task<IActionResult> RecuperarSenha([FromBody] Usuario usuario)
        {
            if (usuario == null || string.IsNullOrEmpty(usuario.Email))
                return BadRequest("O email é obrigatório!");

            // Agora, você pode utilizar o método que envia o e-mail, já utilizando o IUsuarioRepository
            await _enviarEmail.EnviarEmailRecuperacaoSenha(usuario.Email);
            return Ok("Email enviado com sucesso!");
            
        }
    }
}
