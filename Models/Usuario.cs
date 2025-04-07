
namespace GestFinancas_Api.Models;
public class Usuario
{
  public int Id { get; set; }
  public string? Nome { get; set; }
  public string? Email { get; set; }
  public string? Senha { get; set; }
  public DateTime? DataCadastro { get; set; } = DateTime.UtcNow;

  // Propriedades para hash e salt
  public string SenhaHash { get; set; }
  public string SenhaSalt { get; set; }

  public bool IsValid()
  {
    return !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Senha);
  }


}
