using Microsoft.EntityFrameworkCore;
using SIGEDAA.Models;

namespace SIGEDAA.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Equipo> Equipos { get; set; }

        public DbSet<Atleta> Atletas { get; set; }
    }
}