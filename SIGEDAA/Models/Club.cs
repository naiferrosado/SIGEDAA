using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGEDAA.Models
{
    public class Club
    {
        [Key]
        public int IdClub { get; set; }

        // Relación con Asociación
        public int IdAsociacion { get; set; }

        [Required]
        public string NombreClub { get; set; } = string.Empty;

        public string DireccionSede { get; set; } = string.Empty;
        public int? IdEntrenadorPrincipal { get; set; }

        public virtual Entrenador? EntrenadorPrincipal { get; set; }

        [Phone]
        public string Telefono { get; set; } = string.Empty;

        public DateTime FechaInscripcion { get; set; }

        public bool EstadoActivo { get; set; }

        // Relación: Un club puede participar en muchas competencias
        public virtual ICollection<CompetenciaClub> CompetenciasParticipadas { get; set; } = new List<CompetenciaClub>();

    }
}
