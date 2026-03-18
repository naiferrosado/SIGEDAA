using Microsoft.EntityFrameworkCore;
using SIGEDAA.Models;
using System;

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

                // AGREGAR EL USUARIO POR DEFECTO
                b.HasData(new Usuario
                {
                    IdUsuario = 1,
                    NombreCompleto = "Administrador Principal",
                    CorreoElectronico = "admin@fdaa.com",
                    ClaveAcceso = "Admin123",
                    Rol = "Administrador",
                    EstadoActivo = true,
                    FechaRegistro = new DateTime(2026, 3, 18)
                });
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

                // Seed: asociación de prueba
                b.HasData(new AsociacionProvincial
                {
                    IdAsociacion = 1,
                    NombreAsociacion = "Asociación Provincial de Prueba",
                    Provincia = "Santo Domingo",
                    NombrePresidente = "Presidente Prueba",
                    TelefonoContacto = "8090000000",
                    CorreoContacto = "asociacion@prueba.com",
                    FechaFundacion = new DateTime(2020, 1, 1),
                    CertificacionAlDia = true
                });
            });

            // Club
            modelBuilder.Entity<Club>(b =>
            {
                b.HasKey(c => c.IdClub);
                b.ToTable("Clubes");
                b.Property(c => c.NombreClub).IsRequired();
                b.HasOne<AsociacionProvincial>().WithMany().HasForeignKey(c => c.IdAsociacion).OnDelete(DeleteBehavior.Restrict);

                // Seed: club de prueba (usa IdAsociacion = 1)
                b.HasData(new Club
                {
                    IdClub = 1,
                    IdAsociacion = 1,
                    NombreClub = "Club Prueba",
                    DireccionSede = "Calle Prueba 123",
                    NombreDirector = "Director Prueba",
                    Telefono = "8091111111",
                    FechaInscripcion = new DateTime(2024, 1, 1),
                    EstadoActivo = true
                });
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