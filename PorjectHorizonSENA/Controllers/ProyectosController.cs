using Archive;
using Archive.Implementacion;
using Archive.Interfaz;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Persistence.Services.Interfaces;
using Services.Services.Interfaces;

public class ProyectosController : Controller
{
    private readonly IUsuarioService _usuarioService;
    private readonly IProyectoService _proyectoService;
    private readonly FilesManager _filesManager;

    public ProyectosController(IUsuarioService usuarioService, IProyectoService proyectoService, FilesManager filesManager)
    {
        _usuarioService = usuarioService;
        _proyectoService = proyectoService;
        _filesManager = new FilesManager(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Proyectos"));
    }

     [HttpGet]
    public IActionResult Proyectos()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SubirProyecto(int usuarioId, IFormFile archivoZip, string nombreProyecto, string descripcionProyecto)
    {
        var usuarioIdClaim = User.FindFirst("UsuarioId");
        if (usuarioIdClaim == null || !int.TryParse(usuarioIdClaim.Value, out int userId))
        {
            return Unauthorized(); // No se encontró el claim del ID de usuario o no es válido
        }

        var usuario = await _usuarioService.GetUsuarioById(userId);
        if (usuario == null)
        {
            return NotFound(); // Usuario no encontrado
        }

        if (archivoZip != null && archivoZip.Length > 0)
        {
            var targetFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Proyectos");
            var carpetaDescomprimida = await _filesManager.SaveAndExtractFolderAsync(archivoZip, targetFolderPath);

            if (carpetaDescomprimida != null)
            {
                var nuevoProyecto = new Proyectos
                {
                    ArchivoProyecto = carpetaDescomprimida, // Guardar la ruta de la carpeta descomprimida
                    DescripcionProyecto = descripcionProyecto,
                    NombreProyecto = nombreProyecto,
                    IdUsuario = userId,
                    Nombres = usuario.Nombres,
                    PrimerApellido = usuario.PrimerApellido,
                    CtdLikes = 0,
                    CtdCalificaciones = 0,
                    CtdComentarios = 0,
                    CtdFeedbacks = 0,
                    FechaDeCreacion = DateTime.Now,
                    FechaDeModificacion = DateTime.Now,
                    IdTipoAportante = 3 // ID del tipo de aportante "encargado"
                };

                try
                {
                    // Agregar el nuevo proyecto directamente a través del servicio
                    await _proyectoService.CreateProyectoAsync(nuevoProyecto);

                    return RedirectToAction("Proyectos");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al guardar el proyecto en la base de datos: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"InnerException: {ex.InnerException.Message}");
                    }
                    return StatusCode(500, "Error interno al guardar el proyecto.");
                }
            }
        }

        return BadRequest("No se proporcionó ningún archivo ZIP válido para subir.");
    }


    [HttpPost]
    public async Task<IActionResult> AgregarComentario(int idProyecto, string textoComentario)
    {
        // Guardar el comentario en la base de datos
        await _proyectoService.AgregarComentario(idProyecto, textoComentario);

        // Redireccionar de vuelta a la vista del proyecto
        return RedirectToAction("Index", "File");
    }


}
