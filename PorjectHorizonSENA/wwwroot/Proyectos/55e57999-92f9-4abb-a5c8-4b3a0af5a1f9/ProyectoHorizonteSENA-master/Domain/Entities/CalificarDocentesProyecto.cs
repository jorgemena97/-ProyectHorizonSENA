using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class CalificarDocentesProyecto
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id{ get; set; }
    public int IdUsuario { get; set; }
    public int IdProyecto { get; set; }
    public string Feedback { get; set; }
    public bool CalificarDado { get; set; }
    public DateTime FechaDeCreacion { get; set; }

    [ForeignKey("IdUsuario")]
    public virtual Usuario Usuario { get; set; }
    [ForeignKey("IdProyecto")]
    public virtual Proyectos Proyecto { get; set; }
}
