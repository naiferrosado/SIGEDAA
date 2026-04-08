using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGEDAA.Models
{
    public class Atleta
    {
        [Key]
        public int IdAtleta { get; set; }

        

        [Required]
        public string Nombres { get; set; } = string.Empty;

        [Required]
        public string Apellidos { get; set; } = string.Empty;

        public DateTime FechaNacimiento { get; set; }

        public string Genero { get; set; } = string.Empty;

        public decimal EstaturaCm { get; set; }

        public decimal PesoKg { get; set; }

        public string TipoSangre { get; set; } = string.Empty;
        public bool EstadoActivo { get; set; } = true;
        public int IdClub { get; set; }

        [ForeignKey("IdClub")]
        public virtual Club? Club { get; set; } // <--- ESTE ES EL PUENTE QUE FALTA

    }
}
