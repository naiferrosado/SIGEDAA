using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGEDAA.Models
{
    public class Entrenador
    {
        [Key]
        public int IdEntrenador { get; set; }

        
        public int? IdClub { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombres { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(100)]
        public string Apellidos { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Especialidad { get; set; }

        [Range(0, 60)]
        public int AniosExperiencia { get; set; }

        [Phone]
        public string? Telefono { get; set; }

        [EmailAddress]
        public string? Correo { get; set; }

        public bool EstadoActivo { get; set; } = true;

       
        [ForeignKey("IdClub")]
        public virtual Club? Club { get; set; }

    }
}
