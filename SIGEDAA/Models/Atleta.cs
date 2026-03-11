namespace SIGEDAA.Models
{
    public class Atleta
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public int EquipoId { get; set; }

        public Equipo Equipo { get; set; }
    }
}
