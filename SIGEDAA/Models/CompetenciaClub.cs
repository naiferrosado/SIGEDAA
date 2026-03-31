using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGEDAA.Models
{
    public class CompetenciaClub
    {
        [Key]
        public int IdInscripcion { get; set; }

        [Required]
        public int IdCompetencia { get; set; }

        [Required]
        public int IdClub { get; set; }

        public DateTime FechaInscripcion { get; set; } = DateTime.Now;

        public string EstadoInscripcion { get; set; } // Ej. "Confirmado", "Pendiente"

        // Propiedades de navegación
        [ForeignKey("IdCompetencia")]
        public virtual Competencia Competencia { get; set; }

        [ForeignKey("IdClub")]
        public virtual Club Club { get; set; }
    }
}