using System.ComponentModel.DataAnnotations;

namespace SIGEDAA.Models
{
    public class Competencia
    {
        [Key]
        public int IdCompetencia { get; set; }
        // Relación: Un torneo tiene muchos clubes inscritos
        public virtual ICollection<CompetenciaClub> ClubesInscritos { get; set; } = new List<CompetenciaClub>();
        [Required]
        public string NombreEvento { get; set; } = string.Empty;

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public string Sede { get; set; } = string.Empty;

        public string Nivel { get; set; } = string.Empty; // Nacional, Provincial, Invitacional

        public string Estado { get; set; } = string.Empty; // Programada, EnCurso, Finalizada

        public string Observaciones { get; set; } = string.Empty;
    }
}
