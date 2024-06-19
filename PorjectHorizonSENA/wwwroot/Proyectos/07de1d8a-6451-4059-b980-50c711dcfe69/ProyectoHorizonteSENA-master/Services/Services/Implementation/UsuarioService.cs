using Authentication;
using Domain.Entities.Interfaces;
using Domain.Entities;
using Persistence.Repository.Interfaces;
using Persistence.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Services.Implementation;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    
    public UsuarioService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
        
    }

    public async Task<IUsuario?> CreateUsuario(IUsuario usuario) => await _usuarioRepository.CreateUsuarioAsync(usuario);

    public async Task<IUsuario?> ValidateLogin(string email, string password)
    {
        IUsuario? usuario = await GetUsuarioByEmail(email);

        bool isCorrectUser = LoginManager
                                .ValidateLogin(email, password, usuario.Correo, usuario.Contrasena);

        if (isCorrectUser)
        {
            return usuario;
        }

        return null;
    }

    public async Task ResetPassword(IUsuario usuario)
    {
        await _usuarioRepository.ResetPassword(usuario);
    }

    public async Task<IUsuario?> GetUsuarioByEmail(string email)
    {
        return await _usuarioRepository.GetByEmailUsuarioAsync(email);
    }

     public async Task<List<IUsuario>> GetAllUsuarios()
    {
        try
        {
            var usuarios = await _usuarioRepository.GetAllUsuariosAsync();
            return usuarios?.ToList() ?? new List<IUsuario>();
        }
        catch (Exception ex)
        {
       
            return new List<IUsuario>();
        }
    }

    public async Task<bool> UpdateUsuarioRol(IUsuario usuario)
    {
        return await _usuarioRepository.UpdateUsuarioRolAsync(usuario);
    }


}
