using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestFinancas_Api.Dtos
{
  public class RedefinirSenhaDto
  {
    public string Token { get; set; }
    public string NovaSenha { get; set; }
  }
}