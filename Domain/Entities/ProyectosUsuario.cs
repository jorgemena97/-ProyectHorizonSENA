using Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;

namespace Domain.Entities
{
    public class ProyectosUsuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ProyectoId { get; set; }
        public int UsuarioId { get; set; }
        public string Rol { get; set; } // Ejemplo: Líder de proyecto, Aportante, etc.

        // Propiedad de navegación hacia el Proyecto correspondiente
        [ForeignKey("ProyectoId")]
        public virtual Proyectos Proyecto { get; set; }

        // Propiedad de navegación hacia el Usuario correspondiente
        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }
    }
}
