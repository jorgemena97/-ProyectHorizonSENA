using Domain.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Comentarios : IComentarios
    {
       
            public int Id { get; set; }
            public int IdProyecto { get; set; } // Id del Proyecto asociado al comentario
            public int IdUsuario { get; set; }
            public string TextoComentario { get; set; }
            public DateTime FechaComentario { get; set; }

            // Propiedad de navegación
            public Proyectos Proyectos { get; set; }
        
    }
}
