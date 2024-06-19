using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using PorjectHorizonSENA.Models;
using System.Diagnostics;
using System.Security.Claims;



namespace PorjectHorizonSENA.Controllers;


public class HomeController : Controller
{
    [Authorize]

    public IActionResult Index()
    {
        // Verifica si el usuario está autenticado
        if (User.Identity.IsAuthenticated)
        {
            // Obtén el rol del usuario desde las reclamaciones
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var nombre = User.FindFirst(ClaimTypes.Name)?.Value;
            var Apellido = User.FindFirst(ClaimTypes.Surname)?.Value;
            var fotoPerfil = User.FindFirst("FotoPerfil")?.Value;

            // Utiliza el rol del usuario para determinar qué vista parcial de menú mostrar
            switch (role)
            {
                case "Visitante":
                    ViewData["MenuPartial"] = "_Menu_Visitante";
                    break;
                case "Aprendiz":
                    ViewData["MenuPartial"] = "_Menu_Estudiantes";
                    break;
                case "Docente":
                    ViewData["MenuPartial"] = "_Menu_Instructor";
                    break;
                case "Admin":
                    ViewData["MenuPartial"] = "_Menu_Administrador";
                    break;
                default:
                    // Maneja casos donde el rol no está definido o no se proporciona
                    // Redirige directamente a la página de inicio de sesión si el usuario no tiene un rol válido
                    return RedirectToAction("Login");
            }
        }
        else
        {
            // Si el usuario no está autenticado, redirige directamente a la página de inicio de sesión
            return RedirectToAction("Login");
        }

        return View();
    }




    public IActionResult Privacy()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]

    public async Task<IActionResult> LogOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Login", "Access");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}