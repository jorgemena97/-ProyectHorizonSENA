using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using Domain.Entities;
using Persistence.Services.Interfaces;
using Archive;
using Services.Services.Interfaces;
using System.Security.Claims;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Archive.Implementacion;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using PorjectHorizonSENA.Models;

namespace PorjectHorizonSENA.Controllers
{
    public class PerfilController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly FilesManager _filesManager;
        private readonly IProyectoService _proyectoService;
        public PerfilController(IUsuarioService usuarioService, IProyectoService proyectoService)
        {
            _usuarioService = usuarioService;
            _filesManager = new FilesManager(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images"));
            _proyectoService = proyectoService;
        }


        [HttpPost]
        public async Task<IActionResult> SubirFoto(int usuarioId, IFormFile foto)
        {
            var usuario = await _usuarioService.GetUsuarioById(usuarioId);
            if (usuario == null)
            {
                return NotFound();
            }

            if (foto == null || foto.Length == 0)
            {
                ModelState.AddModelError("foto", "Por favor selecciona un archivo.");
                return View("MiPerfil", usuario);
            }

            try
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(foto.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("foto", "El archivo seleccionado no es una imagen válida.");
                    return View("MiPerfil", usuario);
                }

                // Eliminar la foto anterior si existe
                if (!string.IsNullOrEmpty(usuario.FotoPerfil))
                {
                    await _filesManager.DeleteFileIfExistsAsync(usuario.FotoPerfil);
                }

                // Guardar la nueva imagen y obtener el nombre del archivo guardado
                var fileName = await _filesManager.SaveFileAsync(foto);

                // Actualizar la propiedad FotoPerfil del usuario con la nueva imagen
                usuario.FotoPerfil = fileName;

                // Actualizar el usuario en la base de datos
                await _usuarioService.UpdateUsuario(usuario);

                var identity = (ClaimsIdentity)User.Identity;
                var fotoPerfilClaim = identity.FindFirst("FotoPerfil");
                if (fotoPerfilClaim != null)
                {
                    identity.RemoveClaim(fotoPerfilClaim); // Eliminar la reclamación anterior si existe
                }
                identity.AddClaim(new Claim("FotoPerfil", usuario.FotoPerfil)); // Agregar la nueva reclamación de la foto de perfil
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                // Redirigir de vuelta a la acción "MiPerfil" después de subir la foto
                return RedirectToAction("Perfil");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("foto", $"Error al subir la foto: {ex.Message}");
                return View("MiPerfil", usuario);
            }
        }

        private async Task<(Usuario usuario, List<FileModel> fileModels)> PrepareUsuarioConProyectos(int id)
        {
            var usuario = await _usuarioService.GetUsuarioById(id);
            if (usuario == null)
            {
                return (null, null); // Si no se encuentra el usuario, devolver null
            }

            var proyectosUsuario = await _proyectoService.GetProyectosByUsuarioId(id); // Obtener los proyectos asociados al usuario
            usuario.Proyectos = proyectosUsuario; // Asegúrate de que la propiedad 'Proyectos' existe y está en plural

            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/proyectos");
            var fileModels = new List<FileModel>();

            foreach (var proyecto in proyectosUsuario)
            {
                var projectPath = Path.Combine(rootPath, proyecto.ArchivoProyecto);
                if (Directory.Exists(projectPath))
                {
                    // Si el directorio del proyecto aún existe, procede con la obtención de su estructura
                    var projectModel = await GetDirectoryStructure(projectPath, proyecto);
                    fileModels.Add(projectModel);
                }
                else
                {
                    // Si el directorio del proyecto no existe, puedes manejar esta situación aquí, como registrar un mensaje de advertencia
                    Console.WriteLine($"El directorio del proyecto '{proyecto.ArchivoProyecto}' no existe.");
                }
            }

            return (usuario, fileModels); // Devolver el usuario con sus proyectos asociados y los modelos de archivo
        }

        [HttpGet]
        public async Task<IActionResult> Perfil (int? id)
        {
            var userIdClaim = User.FindFirst("UsuarioId");
            if (userIdClaim == null)
            {
                return Unauthorized(); // Si no se encuentra el reclamo 'UsuarioId', devolver 401
            }

            var authenticatedUserId = int.Parse(userIdClaim.Value);
            var usuarioId = id ?? authenticatedUserId;

            var (usuario, fileModels) = await PrepareUsuarioConProyectos(usuarioId);
            if (usuario == null)
            {
                return NotFound(); // Si no se encuentra el usuario, devolver 404
            }

            ViewData["FileModels"] = fileModels; // Pasar los modelos de archivos a la vista
            ViewData["AuthenticatedUserId"] = authenticatedUserId; // Pasar el ID del usuario autenticado a la vista

            return View(usuario); // Pasa el usuario con sus proyectos asociados a la vista 'Perfil.cshtml'
        }


        private async Task<FileModel> GetDirectoryStructure(string path, Proyectos proyecto, bool isRootProjects = false)
        {
            var dirInfo = new DirectoryInfo(path);
            var fileModel = new FileModel
            {
                Name = dirInfo.Name,
                Path = dirInfo.FullName,
                IsDirectory = true,
                NombreProyecto = proyecto.NombreProyecto,
         
           //IdUsuario = proyecto.IdUsuario ?? 0
            };
            var firstSubDirectory = dirInfo.GetDirectories().FirstOrDefault();
            if (firstSubDirectory != null)
            {
                fileModel.Name = firstSubDirectory.Name;
            }

            foreach (var directory in dirInfo.GetDirectories())
            {
                var childModel = await GetDirectoryStructure(directory.FullName, proyecto);
                fileModel.Children.Add(childModel);
            }

            foreach (var file in dirInfo.GetFiles())
            {
                var fileChildModel = new FileModel
                {
                    Name = file.Name,
                    Path = file.FullName,
                    IsDirectory = false
                };
                if (proyecto != null)
                {
                    
                    fileChildModel.NombreProyecto = proyecto.NombreProyecto;
                     
                }
                fileModel.Children.Add(fileChildModel);
            }

            return fileModel;
        }
        

    }
}
