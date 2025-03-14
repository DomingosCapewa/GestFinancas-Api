

public class Usuario
{
  public int Id { get; set; }
  public string? Nome { get; set; }
  public string? Email { get; set; }
  public string? Senha { get; set; }

  public bool IsValid()
  {
    return !string.IsNullOrEmpty(Nome) && !string.IsNullOrEmpty(Email);
  }
}
