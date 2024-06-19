using Domain.Entities;
using Domain.Entities.Interfaces;

namespace Persistence.Repository.Interfaces;

public interface IUsuarioRepository : IRepositoryBase<Usuario>
{
    Task<IUsuario?> CreateUsuarioAsync(IUsuario usuario);
    Task<IEnumerable<IUsuario>> GetAllUsuariosAsync();
    Task<IUsuario?> GetByEmailUsuarioAsync(string email);
    Task<string> UpdateUsuarioAsync(IUsuario usuario);
    Task<string> DeleteUsuarioAsync(IUsuario usuaario);
    Task<IUsuario?> ResetPassword(IUsuario usuario);
    Task<bool> UpdateUsuarioRolAsync(IUsuario usuario);
}
