using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Domain.Entities.Interfaces;
using Domain.Entities;
using Persistence.Repository.Interfaces;
using System.Linq.Expressions;
namespace Persistence.Repository.Implementation;

public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
{


    public UsuarioRepository(HorizonteDBContext context)
    : base(context) { }

    public async Task<Usuario?> GetUsuarioByIdAsync(int id)
    {
        return await _context.Usuario.FindAsync(id);
    }

    public async Task<IUsuario?> CreateUsuarioAsync(IUsuario usuario)
    {

        Usuario usuarioCrear = (Usuario)usuario;

        await CreateAsync(usuarioCrear);

        return await GetByEmailUsuarioAsync(usuario.Correo);
    }

    public async Task<bool> ExisteUsuarioConCorreo(string correo)
    {
        // verifica si existe un usuario con el correo dado
        return await _context.Usuario.AnyAsync(u => u.Correo == correo);
    }
    public async Task<bool> ExisteUsuarioConCedula(string cedula)
    {
        // Verifica si existe algún usuario con la misma cédula en la base de datos
        return await _context.Usuario.AnyAsync(u => u.NumeroDeDocumento == cedula);
    }
    public Task<string> DeleteUsuarioAsync(IUsuario usuaario)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<IUsuario>> GetAllUsuariosAsync()
    {

        return await _context.Usuario.ToListAsync();
    }

    public async Task<Usuario?> GetByEmailUsuarioAsync(string email)
    {
       
            return await _context.Usuario
                .Include(u => u.Rol)
               .FirstOrDefaultAsync(u => u.Correo == email);
        
    }

    public async Task<IUsuario?> ResetPassword(IUsuario usuario)
        {
            IUsuario? user = await GetByEmailUsuarioAsync(usuario.Correo);

            if (user != null)
            {
                user.Contrasena = usuario.Contrasena;
                _context.Usuario.Update((Usuario)user);

                await _context.SaveChangesAsync();
            }

            return user;
        }

        public Task<string> UpdateUsuarioAsync(IUsuario usuario)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateUsuarioRolAsync(IUsuario usuario)
        {
            try
            {
                var entity = await _context.Usuario.FindAsync(usuario.UsuarioId);

                if (entity == null)
                {
                    return false;
                }

                entity.IdRol = usuario.IdRol;

                _context.Entry(entity).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateException ex)
            {
                // Maneja la excepción según tus necesidades
                return false;
            }



        }



}
