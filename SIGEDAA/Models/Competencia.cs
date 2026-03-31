using System.ComponentModel.DataAnnotations;

namespace SIGEDAA.Models
{
    public class Competencia
    {
        [Key]
        public int IdCompetencia { get; set; }
        // Relación: Un torneo tiene muchos clubes inscritos
        public virtual ICollection<CompetenciaClub> ClubesInscritos { get; set; }
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
