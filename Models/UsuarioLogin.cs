using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestFinancas_Api.Models
{
  public class UsuarioLogin
  {
    public string Email { get; set; }
    public string Senha { get; set; }

    public UsuarioLogin()
    {
      Email = string.Empty;
      Senha = string.Empty;
    }
  }

}