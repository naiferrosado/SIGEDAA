using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGEDAA.Models
{
    public class CompetenciaDisciplina
    {
        [Key]
        public int IdCompetenciaDisciplina { get; set; }

        [Required]
        public int IdCompetencia { get; set; }

        [Required]
        public int IdDisciplina { get; set; }

        // Propiedades de navegación
        [ForeignKey("IdCompetencia")]
        public virtual Competencia? Competencia { get; set; }

        [ForeignKey("IdDisciplina")]
        public virtual Disciplina? Disciplina { get; set; }
    }
}