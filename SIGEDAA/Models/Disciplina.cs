using System.ComponentModel.DataAnnotations;

namespace SIGEDAA.Models
{
    public class Disciplina
    {
        [Key]
        public int IdDisciplina { get; set; }

        [Required]
        public string NombreDisciplina { get; set; } = string.Empty;

        public string Tipo { get; set; } = string.Empty; // Pista o Campo

        public string TipoMedicion { get; set; } = string.Empty; // Tiempo o Distancia

        public string GeneroPermitido { get; set; } = string.Empty; // Masculino, Femenino, Mixto

        public bool EsRelevo { get; set; }

        public string CategoriaRecomendada { get; set; } = string.Empty;

        public string DescripcionReglas { get; set; } = string.Empty;
    }
}
