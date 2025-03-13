
using Microsoft.EntityFrameworkCore;

public class UsuarioDbContext : DbContext
{
  public UsuarioDbContext(DbContextOptions<UsuarioDbContext> options) : base(options) { }

  public DbSet<Usuario> Usuarios { get; set; }
}

public class Usuario
{
  public int Id { get; set; }
  public string Nome { get; set; }
  public string Email { get; set; }
}
