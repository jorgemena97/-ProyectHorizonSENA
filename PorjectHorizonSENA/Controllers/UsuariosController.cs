using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Persistence.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.IO;

[Authorize]
public class UsuariosController : Controller
{
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;

    }

    public async Task<IActionResult> Aprendices(string documentoBusqueda)
    {
        var usuarios = await _usuarioService.GetAllUsuarios();

        // Filtrar por número de documento si se proporciona en la consulta
        if (!string.IsNullOrEmpty(documentoBusqueda))
        {
            usuarios = usuarios.Where(u => u.NumeroDeDocumento.Contains(documentoBusqueda)).ToList();
        }

        ViewBag.DocumentoBusqueda = documentoBusqueda;

        return View(usuarios.Cast<Usuario>().ToList());
    }


    public async Task<IActionResult> Instructores(string documentoBusqueda)
    {
        var usuarios = await _usuarioService.GetAllUsuarios();

        // Filtrar por número de documento si se proporciona en la consulta
        if (!string.IsNullOrEmpty(documentoBusqueda))
        {
            usuarios = usuarios.Where(u => u.NumeroDeDocumento.Contains(documentoBusqueda)).ToList();
        }

        ViewBag.DocumentoBusqueda = documentoBusqueda;

        return View(usuarios.Cast<Usuario>().ToList());
    }

 


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ActualizarRolAprendiz(int usuarioId, int IdRol)
    {
        try
        {
            var usuario = new Usuario { UsuarioId = usuarioId, IdRol = IdRol };
            await _usuarioService.UpdateUsuarioRol(usuario);
            return RedirectToAction("Aprendices", "Usuarios");
        }
        catch (Exception ex)
        {

            ModelState.AddModelError(string.Empty, "Error al actualizar el rol del usuario.");
            return View("Aprendices", "Usuarios");
        }
    }



}



