using System.ComponentModel.DataAnnotations;

namespace SIGEDAA.Models
{
    public class Competencia
    {
        [Key]
        public int IdCompetencia { get; set; }

        [Required]
        public string NombreEvento { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public string Sede { get; set; }

        public string Nivel { get; set; } // Nacional, Provincial, Invitacional

        public string Estado { get; set; } // Programada, EnCurso, Finalizada

        public string Observaciones { get; set; }
    }
}
