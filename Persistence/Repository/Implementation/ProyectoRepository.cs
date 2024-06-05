using Domain.Entities;
using Domain.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Repository.Interfaces;
using System.Threading.Tasks;

namespace Persistence.Repository.Implementation
{
    public class ProyectoRepository : RepositoryBase<Proyectos>, IProyectoRepository
    {


        public ProyectoRepository(HorizonteDBContext context)
            : base(context)
        {
        }
        public async Task<Proyectos> GetProyectoById(int id)
        {
            return await _context.Proyectos.FirstOrDefaultAsync(p => p.IdProyectos == id);
        }
      
        public async Task AddComentario(Comentarios comentario)
        {
            _context.Comentarios.Add(comentario);
            await _context.SaveChangesAsync();
        }
        public async Task<Proyectos> GetByPathAsync(string path)
        {
            // Ajusta esto según la estructura de tu base de datos y cómo almacenas las rutas de los proyectos
            return await _context.Proyectos
                .Where(p => p.ArchivoProyecto == path)
                .FirstOrDefaultAsync();
        }
        public async Task<Proyectos> CreateProyectoAsync(Proyectos proyecto)
        {
            await _context.Proyectos.AddAsync(proyecto);
            await _context.SaveChangesAsync();
            return proyecto;
        }


        public async Task<List<Proyectos>> GetProyectosByUsuarioId(int IdUsuario)
        {
            return await _context.Proyectos
                .Where(p => p.IdUsuario == IdUsuario)
                .ToListAsync();
        }
        public async Task<Proyectos> ObtenerProyectoPorId(int proyectoId)
        {
            return await _context.Proyectos.FindAsync(proyectoId);
        }

        public async Task<IEnumerable<Proyectos>> ObtenerTodosLosProyectos()
        {
            return await _context.Proyectos.ToListAsync();
        }

        public async Task ActualizarProyecto(Proyectos proyecto)
        {
            _context.Proyectos.Update(proyecto);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarProyecto(int proyectoId)
        {
            var proyecto = await _context.Proyectos.FindAsync(proyectoId);
            if (proyecto != null)
            {
                _context.Proyectos.Remove(proyecto);
                await _context.SaveChangesAsync();
            }
        }


    }
}
