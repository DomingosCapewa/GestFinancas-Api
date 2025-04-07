using System.Net;
using System.Net.Mail;
using System.Text;
using GestFinancas_Api.Models;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using GestFinancas_Api.Data;

namespace GestFinancas_Api.Helper
{
    public class EnviarEmail
    {
        private readonly string _emailRemetente;
        private readonly string _senha;
        private readonly IUsuarioRepository _usuarioRepository;

        public EnviarEmail(IConfiguration configuration, IUsuarioRepository usuarioRepository)
        {
            _emailRemetente = configuration["EmailSettings:EmailRemetente"];
            _senha = configuration["EmailSettings:Senha"];
            _usuarioRepository = usuarioRepository;
        }

        public async Task EnviarEmailRecuperacaoSenha(string email)
        {
            var usuario = await _usuarioRepository.ObterUsuarioPorEmailAsync(email);

            if (usuario == null)
            {
                Console.WriteLine("Usuário não encontrado!");
                return;
            }

            string nomeUsuario = usuario.Nome;

            if (string.IsNullOrEmpty(email))
            {
                Console.WriteLine("Email não informado!");
                return;
            }

            MailMessage mailMessage = new MailMessage(_emailRemetente, email);
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

            try
            {
                mailMessage.Subject = "Recuperação de senha";
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = $@"
                    <H1>Olá, {nomeUsuario}!</H1><hr>
                    <br>
                    <p>Recebemos uma solicitação para redefinir sua senha no <strong>GestFinancas</strong>. Para criar uma nova senha, clique no link abaixo:</p>
                    <p><a href='http://localhost:4200/resetarSenha' class='button button--green' target='_blank'>Redefinir senha</a></p>
                    <p>Se você não fez essa solicitação, ignore este e-mail. Sua conta permanecerá segura.</p>
                    <p>Este link é válido por 24 horas.</p>
                    <p>Se precisar de ajuda, entre em contato com nosso suporte.</p>
                    <br><hr>
                    <p>Atenciosamente,</p>
                    <p><strong>Equipe GestFinanças</strong></p>
                ";

                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(_emailRemetente, _senha);
                smtpClient.EnableSsl = true;

                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao enviar email: " + ex.Message);
            }
        }
    }
}
