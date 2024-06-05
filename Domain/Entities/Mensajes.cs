using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{

    public class Mensajes
    {
        public int Id { get; set; }
        public int RemitenteId { get; set; }
        public int DestinatarioId { get; set; }
        [Required]
        public string Contenido { get; set; }
        [Required]
        public DateTime FechaEnvio { get; set; }

        // Propiedades de navegación para las relaciones con la clase Usuario
        public virtual Usuario Remitente { get; set; }
        public virtual Usuario Destinatario { get; set; }
    }

}
