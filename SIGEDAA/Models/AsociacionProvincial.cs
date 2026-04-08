using System.ComponentModel.DataAnnotations;

namespace SIGEDAA.Models
{
    public class AsociacionProvincial
    {
        [Key]
        public int IdAsociacion { get; set; }

        [Required]
        public string NombreAsociacion { get; set; } = string.Empty;

        [Required]
        public string Provincia { get; set; } = string.Empty;

        public string NombrePresidente { get; set; } = string.Empty;

        [Phone]
        public string TelefonoContacto { get; set; } = string.Empty;

        [EmailAddress]
        public string CorreoContacto { get; set; } = string.Empty;

        public DateTime FechaFundacion { get; set; }

        public bool CertificacionAlDia { get; set; }
        public bool EstadoActivo { get; set; } = true;
    }
}
