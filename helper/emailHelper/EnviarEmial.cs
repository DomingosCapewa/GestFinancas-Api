using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using GestFinancas_Api.Models;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;


namespace GestFinancas_Api.Helper
{
  public class EnviarEmail
  {
    private readonly string _emailRemetente;
    private readonly string _senha;
    private readonly string _emailDestinatario;
    private readonly string _nomeUsuario;
    // private readonly string _linkRecuperacaoSenha;

    public EnviarEmail(IConfiguration configuration)
    {
      _emailRemetente = configuration["EmailSettings:EmailRemetente"];
      _senha = configuration["EmailSettings:Senha"];
      _emailDestinatario = configuration["EmailSettings:EmailDestinatario"];
      _nomeUsuario = configuration["EmailSettings:NomeUsuario"];
    }

    public void EnviarEmailRecuperacaoSenha(string email)
    {
     
      var usuario = new Usuario();

      if (usuario == null)
      {
        Console.WriteLine("Usuário não encontrado!");
        return;
      }
      
        // Console.WriteLine("Email: " + _emailRemetente);
        // Console.WriteLine("Senha: " + _senha);

        string nomeUsuario = usuario.Nome;

        Console.WriteLine("Email: " + email);
        Console.WriteLine("Nome:" + nomeUsuario);

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
                <link rel='stylesheet' href='../../../GestFinancas/src/assets/Styles/css/email.scss'>
                  <p>Recebemos uma solicitação para redefinir sua senha no <strong>GestFinancas</strong>. Para criar uma nova senha, clique no link abaixo:</p>
                <p><a href='http://localhost:4200/resetarSenha'  class='button button--green' target='_blank'>Redefinir senha</a></p>
                <p>Se você não fez essa solicitação, ignore este e-mail. Sua conta permanecerá segura.</p>
                <p>Este link é válido por 24 horas.</p>
                <p>Se precisar de ajuda, entre em contato com nosso suporte.</p>
                <br>
                <br>
                <hr>
                <p>Atenciosamente,</p>
                <p><strong>Equipe GestFinanças</strong></p>
            ";

          smtpClient.UseDefaultCredentials = false;
          smtpClient.Credentials = new NetworkCredential(_emailRemetente, _senha);
          smtpClient.EnableSsl = true;

          smtpClient.Send(mailMessage);
          //Console.WriteLine("Email enviado com sucesso!");
        }
        catch (Exception ex)
        {
          Console.WriteLine("Erro ao enviar email: " + ex.Message);
        }
      }
    
    }

  }