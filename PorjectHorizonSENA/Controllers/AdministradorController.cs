using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Persistence.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Http.Extensions;

//[Authorize]
public class AdministradorController : Controller
{
    private readonly IUsuarioService _usuarioService;
    private readonly IConverter _converter;
    public AdministradorController (IUsuarioService usuarioService, IConverter converter)
    {
        _usuarioService = usuarioService;
        _converter = converter;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult VistaParaPDF()
    {
        return View();
    }
    public IActionResult MostrarPDFenPagina()
    {
        string pagina_actual = HttpContext.Request.Path;
        string url_pagina = HttpContext.Request.GetEncodedUrl();
        url_pagina = url_pagina.Replace(pagina_actual, "");
        url_pagina = $"{url_pagina}/Administrador/VistaParaPDF";


        var pdf = new HtmlToPdfDocument()
        {
            GlobalSettings = new GlobalSettings()
            {
                PaperSize = PaperKind.A4,
                Orientation = Orientation.Portrait
            },
            Objects = {
                    new ObjectSettings(){
                        Page = url_pagina
                    }
                }

        };

        var archivoPDF = _converter.Convert(pdf);


        return File(archivoPDF, "application/pdf");
    }

    public IActionResult DescargarPDF()
    {
        string pagina_actual = HttpContext.Request.Path;
        string url_pagina = HttpContext.Request.GetEncodedUrl();
        url_pagina = url_pagina.Replace(pagina_actual, "");
        url_pagina = $"{url_pagina}/Administrador/VistaParaPDF";


        var pdf = new HtmlToPdfDocument()
        {
            GlobalSettings = new GlobalSettings()
            {
                PaperSize = PaperKind.A4,
                Orientation = Orientation.Portrait
            },
            Objects = {
                    new ObjectSettings(){
                        Page = url_pagina
                    }
                }

        };

        var archivoPDF = _converter.Convert(pdf);
        string nombrePDF = "reporte_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf";

        return File(archivoPDF, "application/pdf", nombrePDF);
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
    public async Task<IActionResult> ActualizarRolInstructor(int usuarioId, int IdRol)
    {
        try
        {
            var usuario = new Usuario { UsuarioId = usuarioId, IdRol = IdRol };
            await _usuarioService.UpdateUsuarioRol(usuario);
            return RedirectToAction("Instructores", "Administrador");
        }
        catch (Exception ex)
        {

            ModelState.AddModelError(string.Empty, "Error al actualizar el rol del usuario.");
            return View("Instructores", "Administrador");
        }
    }
}



