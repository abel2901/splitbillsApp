using Microsoft.EntityFrameworkCore;
using SplitBillsApi.Models;

namespace SplitBillsApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<Gasto> Gastos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relação muitos-para-muitos entre Grupo e Usuario
            modelBuilder.Entity<Grupo>()
                .HasMany(g => g.Usuarios)
                .WithMany(u => u.Grupos);
        }
    }
}
