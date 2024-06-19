using Domain.Entities.Interfaces;

namespace Persistence.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<IUsuario?> CreateUsuario(IUsuario usuario);
        Task<IUsuario?> ValidateLogin(string email, string password);
        Task<IUsuario?> GetUsuarioByEmail(string email);
        Task ResetPassword(IUsuario usuario);
        Task<bool> UpdateUsuarioRol(IUsuario usuario);
        Task<List<IUsuario>> GetAllUsuarios();
      
    }
}