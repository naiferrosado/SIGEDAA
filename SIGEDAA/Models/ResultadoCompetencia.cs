using System.ComponentModel.DataAnnotations;

namespace SIGEDAA.Models
{
    public class ResultadoCompetencia
    {
        [Key]
        public int IdResultado { get; set; }

        public int IdCompetencia { get; set; }

        public int IdAtleta { get; set; }

        public int IdDisciplina { get; set; }

        [Required]
        public string MarcaObtenida { get; set; } // Ej. "00:10.55" o "8.25m"

        public int PosicionFinal { get; set; }

        public decimal PuntosOtorgados { get; set; }

        public bool EsRecordNacional { get; set; }
    }
}
