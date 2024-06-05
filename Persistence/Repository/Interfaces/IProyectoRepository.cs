using Domain.Entities;
using Domain.Entities.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistence.Repository.Interfaces
{
    public interface IProyectoRepository
    {
        Task AddComentario(Comentarios comentario);
        Task<Proyectos> CreateProyectoAsync(Proyectos proyecto);
        Task<Proyectos> ObtenerProyectoPorId(int proyectoId);
        Task<IEnumerable<Proyectos>> ObtenerTodosLosProyectos();
        Task ActualizarProyecto(Proyectos proyecto);
        Task EliminarProyecto(int proyectoId);
        Task<Proyectos> GetByPathAsync(string path);
       
    
        Task<List<Proyectos>> GetProyectosByUsuarioId(int IdUsuario);
        // Otros métodos del repositorio...
    }
}
