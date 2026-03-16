using System.ComponentModel.DataAnnotations;

namespace SIGEDAA.Models
{
    public class Atleta
    {
        [Key]
        public int IdAtleta { get; set; }

        public int IdClub { get; set; } // Relación con su club

        [Required]
        public string Nombres { get; set; }

        [Required]
        public string Apellidos { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public string Genero { get; set; }

        public decimal EstaturaCm { get; set; }

        public decimal PesoKg { get; set; }

        public string TipoSangre { get; set; }
    }
}
