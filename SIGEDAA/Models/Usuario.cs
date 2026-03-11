using System.ComponentModel.DataAnnotations;
namespace SIGEDAA.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string Rol { get; set; }
    }
}
