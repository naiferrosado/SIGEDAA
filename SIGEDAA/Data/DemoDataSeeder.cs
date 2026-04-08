using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SIGEDAA.Models;
using SIGEDAA.Services;

namespace SIGEDAA.Data;

public static class DemoDataSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using IServiceScope scope = serviceProvider.CreateScope();
        AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        IPasswordService passwordService = scope.ServiceProvider.GetRequiredService<IPasswordService>();

        await EnsureUserAsync(
            context,
            passwordService,
            "Administrador Principal",
            "admin@fdaa.com",
            "Administrador",
            "Admin123",
            true);

        await EnsureUserAsync(
            context,
            passwordService,
            "Presidente Demo",
            "presidente@sigedaa.local",
            "Presidente",
            "Demo123!",
            true);

        await EnsureUserAsync(
            context,
            passwordService,
            "Juez Demo",
            "juez@sigedaa.local",
            "Juez",
            "Demo123!",
            true);
    }

    private static async Task EnsureUserAsync(
        AppDbContext context,
        IPasswordService passwordService,
        string nombreCompleto,
        string correoElectronico,
        string rol,
        string password,
        bool estadoActivo)
    {
        Usuario? usuario = await context.Usuarios.FirstOrDefaultAsync(u => u.CorreoElectronico == correoElectronico);

        if (usuario is null)
        {
            usuario = new Usuario
            {
                NombreCompleto = nombreCompleto,
                CorreoElectronico = correoElectronico,
                Rol = rol,
                EstadoActivo = estadoActivo,
                FechaRegistro = DateTime.Now
            };

            usuario.ClaveAcceso = passwordService.HashPassword(usuario, password);
            context.Usuarios.Add(usuario);
            await context.SaveChangesAsync();
            return;
        }

        bool updated = false;

        if (!passwordService.IsHashed(usuario.ClaveAcceso))
        {
            usuario.ClaveAcceso = passwordService.HashPassword(usuario, password);
            updated = true;
        }

        if (usuario.Rol != rol)
        {
            usuario.Rol = rol;
            updated = true;
        }

        if (usuario.EstadoActivo != estadoActivo)
        {
            usuario.EstadoActivo = estadoActivo;
            updated = true;
        }

        if (updated)
        {
            await context.SaveChangesAsync();
        }
    }
}
