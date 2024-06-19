using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class ProyectosUsuario
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int IdLiderProyecto { get; set; }
    public int IdUsuarioAportante { get; set; }
    public int IdProyectoUsuario { get; set; }

    [ForeignKey("IdLiderProyecto")]
    public virtual Usuario UsuarioLider { get; set; }
    [ForeignKey("IdUsuarioAportante")]
    public virtual Usuario UsuariosAportantes { get; set; }
    [ForeignKey("IdProyectoUsuario")]
    public virtual Proyectos Proyecto { get; set; }
}
