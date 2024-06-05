using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Interfaces
{
    public interface IComentarios
    {
         int Id { get; set; }
         int IdProyecto { get; set; } // Id del Proyecto asociado al comentario
        int IdUsuario { get; set; }
         string TextoComentario { get; set; }
         DateTime FechaComentario { get; set; }

        // Propiedad de navegación
         Proyectos Proyectos { get; set; }

    }
}
    