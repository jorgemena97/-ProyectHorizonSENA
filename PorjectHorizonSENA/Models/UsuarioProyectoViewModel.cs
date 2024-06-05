using Domain.Entities;

namespace PorjectHorizonSENA.Models
{
    public class UsuarioProyectoViewModel
    {
        public Usuario Usuario { get; set; }
        public List<Proyectos> Proyectos { get; set; }
    }
}
