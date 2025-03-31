
namespace GestFinancas_Api.Models;
public class Usuario
{
  public int Id { get; set; }
  public string? Nome { get; set; }
  public string? Email { get; set; }
  public string? Senha { get; set; }
  public DateTime? DataAtualizacao { get; set; } = DateTime.UtcNow;

  public bool IsValid()
  {
    return !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Senha);
  }


}
