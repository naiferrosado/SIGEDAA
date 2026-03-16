using System.ComponentModel.DataAnnotations;

namespace SIGEDAA.Models
{
    public class AsociacionProvincial
    {
        [Key]
        public int IdAsociacion { get; set; }

        [Required]
        public string NombreAsociacion { get; set; }

        [Required]
        public string Provincia { get; set; }

        public string NombrePresidente { get; set; }

        [Phone]
        public string TelefonoContacto { get; set; }

        [EmailAddress]
        public string CorreoContacto { get; set; }

        public DateTime FechaFundacion { get; set; }

        public bool CertificacionAlDia { get; set; }
    }
}
