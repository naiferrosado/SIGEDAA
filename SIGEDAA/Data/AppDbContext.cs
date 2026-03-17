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
        public DbSet<AsociacionProvincial> AsociacionesProvinciales { get; set; }
        public DbSet<Club> Clubes { get; set; }
        public DbSet<Competencia> Competencias { get; set; }
        public DbSet<Disciplina> Disciplinas { get; set; }
        public DbSet<ResultadoCompetencia> ResultadosCompetencia { get; set; }
        public DbSet<Entrenador> Entrenadores { get; set; }
    

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Usuario: clave personalizada (IdUsuario)
            modelBuilder.Entity<Usuario>(b =>
            {
                b.HasKey(u => u.IdUsuario);
                b.ToTable("Usuarios");
                b.Property(u => u.IdUsuario).HasColumnName("Id");
                b.Property(u => u.NombreCompleto).HasColumnName("Nombre");
                b.Property(u => u.CorreoElectronico).HasColumnName("Email");
                b.Property(u => u.ClaveAcceso).IsRequired();
            });

            // Equipo
            modelBuilder.Entity<Equipo>(b =>
            {
                b.HasKey(e => e.Id);
                b.ToTable("Equipos");
                b.Property(e => e.Nombre).IsRequired();
                b.Property(e => e.Ciudad).IsRequired();
            });

            // Atleta
            modelBuilder.Entity<Atleta>(b =>
            {
                b.HasKey(a => a.IdAtleta);
                b.ToTable("Atletas");
                b.Property(a => a.EstaturaCm).HasPrecision(5, 2);
                b.Property(a => a.PesoKg).HasPrecision(6, 2);

                // Relación con Club si existe
                b.HasOne<Club>().WithMany().HasForeignKey(a => a.IdClub).OnDelete(DeleteBehavior.Restrict);
            });

            // Asociacion
            modelBuilder.Entity<AsociacionProvincial>(b =>
            {
                b.HasKey(x => x.IdAsociacion);
                b.ToTable("AsociacionesProvinciales");
                b.Property(x => x.NombreAsociacion).IsRequired();
                b.Property(x => x.Provincia).IsRequired();
            });

            // Club
            modelBuilder.Entity<Club>(b =>
            {
                b.HasKey(c => c.IdClub);
                b.ToTable("Clubes");
                b.Property(c => c.NombreClub).IsRequired();
                b.HasOne<AsociacionProvincial>().WithMany().HasForeignKey(c => c.IdAsociacion).OnDelete(DeleteBehavior.Restrict);
            });

            // Competencia
            modelBuilder.Entity<Competencia>(b =>
            {
                b.HasKey(c => c.IdCompetencia);
                b.ToTable("Competencias");
                b.Property(c => c.NombreEvento).IsRequired();
            });

            // Disciplina
            modelBuilder.Entity<Disciplina>(b =>
            {
                b.HasKey(d => d.IdDisciplina);
                b.ToTable("Disciplinas");
                b.Property(d => d.NombreDisciplina).IsRequired();
            });

            // ResultadoCompetencia
            modelBuilder.Entity<ResultadoCompetencia>(b =>
            {
                b.HasKey(r => r.IdResultado);
                b.ToTable("ResultadosCompetencia");
                b.HasOne<Competencia>().WithMany().HasForeignKey(r => r.IdCompetencia).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<Atleta>().WithMany().HasForeignKey(r => r.IdAtleta).OnDelete(DeleteBehavior.Restrict);
                b.HasOne<Disciplina>().WithMany().HasForeignKey(r => r.IdDisciplina).OnDelete(DeleteBehavior.Restrict);
            });

            // Entrenador
            modelBuilder.Entity<Entrenador>(b =>
            {
                b.HasKey(e => e.IdEntrenador);
                b.ToTable("Entrenadores");
                b.HasOne<Club>().WithMany().HasForeignKey(e => e.IdClub).OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}