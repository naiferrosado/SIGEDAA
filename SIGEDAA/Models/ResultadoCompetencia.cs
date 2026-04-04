using SIGEDAA.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SIGEDAA.Models;
    public class ResultadoCompetencia
{
    [Key]
    public int IdResultado { get; set; }

    [Required]
    public int IdCompetencia { get; set; }

    [Required]
    public int IdAtleta { get; set; }

    [Required]
    public int IdClub { get; set; } // <--- NUEVO CAMPO

    [Required]
    public int IdDisciplina { get; set; }

    [Required]
    public string MarcaObtenida { get; set; }

    public int PosicionFinal { get; set; }
    public decimal PuntosOtorgados { get; set; }
    public bool EsRecordNacional { get; set; }

    // Propiedades de navegación
    [ForeignKey("IdCompetencia")]
    public virtual Competencia Competencia { get; set; }
    [ForeignKey("IdAtleta")]
    public virtual Atleta Atleta { get; set; }
    [ForeignKey("IdClub")]
    public virtual Club Club { get; set; } // <--- NUEVA NAVEGACIÓN
    [ForeignKey("IdDisciplina")]
    public virtual Disciplina Disciplina { get; set; }
}
