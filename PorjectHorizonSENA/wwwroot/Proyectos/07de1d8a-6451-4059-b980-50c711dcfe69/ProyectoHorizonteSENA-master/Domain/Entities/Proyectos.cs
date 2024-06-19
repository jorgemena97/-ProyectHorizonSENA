using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Proyectos
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int IdProyectosUsuario { get; set; }
    public string? ArchivoProyecto { get; set; }
    public string? DescripcionProyecto { get; set; }
    public string? NombreProyecto { get; set; }
    public int CtdLikes { get; set; }
    public int CtdCalificaciones { get; set; }
    public int CtdComentarios { get; set; }
    public int CtdFeedbacks { get; set; }
    public DateTime FechaDeCreacion { get; set; }
    public DateTime FechaDeModificacion { get; set; }

    [ForeignKey("IdProyectosUsuario")]
    public virtual ProyectosUsuario Usuarios { get; set; }
}
