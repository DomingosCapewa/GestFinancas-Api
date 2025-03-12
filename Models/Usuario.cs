using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestFinancas_Api.Models
{
  public class Usuario
  {
    public string nomeUsuario { get; set; }
    public string emailUsuario { get; set; }
    public string senha { get; set; }


    public Usuario(string nomeUsuario, string emailUsuario, string senha)
    {
      this.nomeUsuario = nomeUsuario ?? throw new ArgumentNullException(nameof(nomeUsuario));
      this.emailUsuario = emailUsuario ?? throw new ArgumentNullException(nameof(emailUsuario));
      this.senha = senha ?? throw new ArgumentNullException(nameof(senha));
    }
  }
}