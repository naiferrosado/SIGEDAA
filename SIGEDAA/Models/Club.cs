using System.ComponentModel.DataAnnotations;

namespace SIGEDAA.Models
{
    public class Club
    {
        [Key]
        public int IdClub { get; set; }

        // Relación con Asociación
        public int IdAsociacion { get; set; }

        [Required]
        public string NombreClub { get; set; }

        public string DireccionSede { get; set; }

        public string NombreDirector { get; set; }

        [Phone]
        public string Telefono { get; set; }

        public DateTime FechaInscripcion { get; set; }

        public bool EstadoActivo { get; set; }

        // Relación: Un club puede participar en muchas competencias
        public virtual ICollection<CompetenciaClub> CompetenciasParticipadas { get; set; }

    }
}
