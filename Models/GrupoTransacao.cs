using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestFinancas_Api.Models
{
  public class GrupoTransacao
  {
    public int? Id { get; set; }
    public int? TransacaoId { get; set; }
    public int? GrupoId { get; set; }

  }
}
