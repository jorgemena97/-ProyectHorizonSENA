using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Interfaces
{
    public interface IProyecto
    {

        int IdProyectos { get; set; }
        string? ArchivoProyecto { get; set; }
        string? DescripcionProyecto { get; set; }
        string? NombreProyecto { get; set; }
        int CtdLikes { get; set; }
        int CtdCalificaciones { get; set; }
        int CtdComentarios { get; set; }
        int CtdFeedbacks { get; set; }
        DateTime FechaDeCreacion { get; set; }
        DateTime FechaDeModificacion { get; set; }


        [ForeignKey("IdUsuario")]
         Usuario Usuario { get; set; }
        
     public ICollection<Comentarios> Comentarios { get; set; }
    }
}
