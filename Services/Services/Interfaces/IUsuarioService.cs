using Domain.Entities;
using Domain.Entities.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistence.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<IUsuario?> CreateUsuario(IUsuario usuario);
        Task<IUsuario?> ValidateLogin(string email, string password);
        Task<IUsuario?> GetUsuarioByEmail(string email);
        Task<Usuario?> GetUsuarioById(int id); // Nuevo método para obtener un usuario por ID
        Task UpdateUsuario(IUsuario usuario); // Nuevo método para actualizar un usuario
        Task ResetPassword(IUsuario usuario);
        Task<bool> UpdateUsuarioRol(IUsuario usuario);
        Task<List<IUsuario>> GetAllUsuarios();
    }
}
