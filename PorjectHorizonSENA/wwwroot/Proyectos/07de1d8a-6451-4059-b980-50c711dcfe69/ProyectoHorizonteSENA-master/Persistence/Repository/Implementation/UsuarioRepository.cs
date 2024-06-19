using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Domain.Entities.Interfaces;
using Domain.Entities;
using Persistence.Repository.Interfaces;
namespace Persistence.Repository.Implementation;

public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(HorizonteDBContext context)
    : base(context) { }

    public async Task<IUsuario?> CreateUsuarioAsync(IUsuario usuario)
    {
        Usuario usuarioCrear = (Usuario)usuario;

        await CreateAsync(usuarioCrear);

        return await GetByEmailUsuarioAsync(usuario.Correo);
    }

    public Task<string> DeleteUsuarioAsync(IUsuario usuaario)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<IUsuario>> GetAllUsuariosAsync()
    {

        return await _context.Usuario.ToListAsync();
    }

    public async Task<IUsuario?> GetByEmailUsuarioAsync(string email)
    {
        return await _context.Usuario
            .Include(u => u.Rol)
            .Include(u => u.Proyecto)
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
