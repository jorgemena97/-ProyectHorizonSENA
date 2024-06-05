using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Interfaces
{
    public interface IMensajes
    {
        int Id { get; set; }
        int RemitenteId { get; set; }

        int DestinatarioId { get; set; }

        string Contenido { get; set; }

        DateTime FechaEnvio { get; set; }
    }
}



