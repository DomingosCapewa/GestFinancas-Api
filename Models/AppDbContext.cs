using Microsoft.EntityFrameworkCore;

namespace GestFinancas_Api.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSet para a entidade Usuario
        public DbSet<Usuario> Usuario { get; set; } = null!;
        public DbSet<Transacao> Transacao { get; set; } = null!;
        public DbSet<Grupo> Grupo { get; set; } = null!;
        public DbSet<GrupoTransacao> GrupoTransacao { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
        }
    }
}
