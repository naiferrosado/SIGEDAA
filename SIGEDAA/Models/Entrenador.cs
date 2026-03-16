using System.ComponentModel.DataAnnotations;

namespace SIGEDAA.Models
{
    public class Entrenador
    {
        [Key]
        public int IdEntrenador { get; set; }

        public int IdClub { get; set; } // Club al que pertenece

        [Required]
        public string Nombres { get; set; }

        [Required]
        public string Apellidos { get; set; }

        public string Especialidad { get; set; } // Ej. Velocidad, Lanzamientos

        public int AniosExperiencia { get; set; }

        [Phone]
        public string Telefono { get; set; }

        [EmailAddress]
        public string Correo { get; set; }
    }
}
