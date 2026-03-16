using System.ComponentModel.DataAnnotations;

namespace SIGEDAA.Models
{
    public class Disciplina
    {
        [Key]
        public int IdDisciplina { get; set; }

        [Required]
        public string NombreDisciplina { get; set; }

        public string Tipo { get; set; } // Pista o Campo

        public string TipoMedicion { get; set; } // Tiempo o Distancia

        public string GeneroPermitido { get; set; } // Masculino, Femenino, Mixto

        public bool EsRelevo { get; set; }

        public string CategoriaRecomendada { get; set; }

        public string DescripcionReglas { get; set; }
    }
}
