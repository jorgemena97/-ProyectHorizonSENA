using Domain.Entities;
using Services.Services.Interfaces;
using System.Threading.Tasks;
using Persistence.Repository.Interfaces;
using Domain.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Services.Services.Implementation
{
    public class ProyectoService : IProyectoService
    {
        private readonly IProyectoRepository _proyectoRepository;

        public ProyectoService(IProyectoRepository proyectoRepository)
        {
            _proyectoRepository = proyectoRepository;
        }

        public async Task AgregarComentario(int idProyecto, string textoComentario)
        {
            // Crear un nuevo objeto Comentarios con el texto proporcionado y la fecha actual
            var comentario = new Comentarios
            {
                IdProyecto = idProyecto,
                TextoComentario = textoComentario,
                FechaComentario = DateTime.Now
            };

            // Guardar el comentario en la base de datos
            await _proyectoRepository.AddComentario(comentario);
        }

        public async Task<List<Proyectos>> GetProyectosByUsuarioId(int IdUsuario)
        {
            return await _proyectoRepository.GetProyectosByUsuarioId(IdUsuario);
        }
        public async Task<Proyectos> GetProyectoByPath(string path)
        {
            return await _proyectoRepository.GetByPathAsync(path);
        }
        public async Task<Proyectos> GetByPathAsync(string path)
        {
            return await _proyectoRepository.GetByPathAsync(path);
        }
        public async Task<Proyectos> CreateProyectoAsync(Proyectos proyecto)
        {
            return await _proyectoRepository.CreateProyectoAsync(proyecto);
        }
        public async Task<string> ObtenerRutaProyectoPorIdUsuario(int idUsuario)
        {
            var proyectos = await _proyectoRepository.GetProyectosByUsuarioId(idUsuario);

            // Supongamos que quieres obtener la ruta del primer proyecto encontrado
            var primerProyecto = proyectos.FirstOrDefault();

            if (primerProyecto != null)
            {
                return primerProyecto.ArchivoProyecto;
            }
            else
            {
                return null; // Manejar el caso si no se encuentra ningún proyecto
            }
        }
    }
}


