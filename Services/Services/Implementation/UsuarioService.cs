using System;
using System.IO;
using System.Threading.Tasks;
using Authentication;
using Domain.Entities.Interfaces;
using Domain.Entities;
using Persistence.Repository.Interfaces;
using Persistence.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Services.Services.Interfaces;


namespace Persistence.Services.Implementation;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UsuarioService(IUsuarioRepository usuarioRepository, IPasswordHasher passwordHasher)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
    }
   
   

    public async Task<Usuario?> GetUsuarioById(int id)
    {
        return await _usuarioRepository.GetUsuarioByIdAsync(id);
    }

    public async Task UpdateUsuario(IUsuario usuario)
    {
        // Realizar una conversión explícita de IUsuario a Usuario, asumiendo que IUsuario es una interfaz que define las propiedades comunes
        if (usuario is Usuario userToUpdate)
        {
            await _usuarioRepository.UpdateAsync(userToUpdate);
        }
        else
        {
            // Manejar el caso en el que la conversión no es válida
            throw new ArgumentException("El objeto IUsuario no es compatible con Usuario.");
        }
    }


    public async Task<IUsuario?> CreateUsuario(IUsuario usuario)
    {
        // Verifica si ya existe un usuario con el correo electrónico proporcionado
        if (await _usuarioRepository.ExisteUsuarioConCorreo(usuario.Correo))
        {
            throw new InvalidOperationException("El usuario con el correo electrónico proporcionado ya existe.");
        }

        // Verifica si ya existe un usuario con el mismo número de cédula
        if (await _usuarioRepository.ExisteUsuarioConCedula(usuario.NumeroDeDocumento))
        {
            throw new InvalidOperationException("Ya existe un usuario con el mismo número de cédula.");
        }

        // Encriptar la contraseña y generar la sal
        string salt = _passwordHasher.GenerateSalt();
        string hashedPassword = _passwordHasher.HashPassword(usuario.Contrasena + salt);

        // Asignar la contraseña encriptada y la sal al usuario
        usuario.Contrasena = hashedPassword;
        usuario.Salt = salt;

        // Si no existe ningún usuario con el mismo correo ni la misma cédula, procede con la creación del nuevo usuario
        await _usuarioRepository.CreateUsuarioAsync(usuario);

        return usuario;
    }

    public async Task<IUsuario?> ValidateLogin(string email, string password)
    {
        // Obtener el usuario por su correo electrónico
        var usuario = await _usuarioRepository.GetByEmailUsuarioAsync(email);

        if (usuario != null)
        {
            // Verificar si la contraseña proporcionada coincide con la contraseña almacenada
            bool isCorrectPassword = _passwordHasher.VerifyPassword(password, usuario.Contrasena, usuario.Salt);

            if (isCorrectPassword)
            {
                // La contraseña es correcta, devuelve al usuario
                return usuario;
            }
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
            // Registra el error para fines de depuración
            Console.WriteLine($"Error al obtener usuarios: {ex.Message}");
            throw; // Relanza la excepción para que sea manejada en un nivel superior si es necesario
        }
    }


    public async Task<bool> UpdateUsuarioRol(IUsuario usuario)
    {
        return await _usuarioRepository.UpdateUsuarioRolAsync(usuario);
    }


}
