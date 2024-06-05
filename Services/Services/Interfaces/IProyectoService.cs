using Domain.Entities;
using Domain.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Interfaces
{
    public interface IProyectoService
    {
        Task AgregarComentario(int idProyecto, string textoComentario);

        Task<List<Proyectos>> GetProyectosByUsuarioId(int usuarioId);

        Task<Proyectos> CreateProyectoAsync(Proyectos proyecto);
        Task<string> ObtenerRutaProyectoPorIdUsuario(int idUsuario);
        Task<Proyectos> GetProyectoByPath(string path);

        Task<Proyectos> GetByPathAsync(string path);

    }
}
