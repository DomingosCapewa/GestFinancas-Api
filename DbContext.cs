using Microsoft.EntityFrameworkCore;
using GestFinancas.Models;
using Usuario = GestFinancas.Models.Usuario;

namespace GestFinancas.Data
{
  public class GestFinancasContext : DbContext
  {
    public GestFinancasContext(DbContextOptions<GestFinancasContext> options)
        : base(options) { }

    public DbSet<Usuario> Usuario { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      // Configurações adicionais de mapeamento de entidades
    }
  }
}
