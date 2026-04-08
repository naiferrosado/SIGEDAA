using System.ComponentModel.DataAnnotations;
namespace SIGEDAA.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required]
        [MaxLength(50)]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string CorreoElectronico { get; set; } = string.Empty;

        [Required]
        public string ClaveAcceso { get; set; } = string.Empty; // En producción debe ser un Hash

        [Required]
        public string Rol { get; set; } = string.Empty; // Ej. Administrador, Juez, Presidente

        public bool EstadoActivo { get; set; }

        public DateTime FechaRegistro { get; set; }
    }
}
