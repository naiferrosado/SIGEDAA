using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace SIGEDAA.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // Busca appsettings.json en el árbol de directorios comenzando desde AppContext.BaseDirectory
            var baseDir = AppContext.BaseDirectory;
            var dir = new DirectoryInfo(baseDir);
            FileInfo settingsFile = null;

            while (dir != null)
            {
                var candidate = Path.Combine(dir.FullName, "appsettings.json");
                if (File.Exists(candidate))
                {
                    settingsFile = new FileInfo(candidate);
                    break;
                }
                dir = dir.Parent;
            }

            if (settingsFile == null)
            {
                throw new FileNotFoundException("No se encontró 'appsettings.json' en los directorios padre. Asegúrate de que exista en la raíz del proyecto.", baseDir);
            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(settingsFile.DirectoryName)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("No se encontró la cadena de conexión 'DefaultConnection' en appsettings.json.");
            }

            optionsBuilder.UseSqlServer(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}