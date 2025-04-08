using Microsoft.EntityFrameworkCore;
using GestFinancas_Api.Models;
using Usuario = GestFinancas_Api.Models.Usuario;

namespace GestFinancas_Api.Data
{
  public class GestFinancasContext : DbContext
  {
    public GestFinancasContext(DbContextOptions<GestFinancasContext> options)
        : base(options) { }

    public DbSet<Usuario> Usuario { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
    }
  }
}
