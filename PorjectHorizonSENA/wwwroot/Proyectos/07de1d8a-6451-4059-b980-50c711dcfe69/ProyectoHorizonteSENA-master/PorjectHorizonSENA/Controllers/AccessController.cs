using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using PorjectHorizonSENA.Models;
using Domain.Entities;
using Domain.Entities.Interfaces;
using Persistence.Services.Implementation;
using Persistence.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace PorjectHorizonSENA.Controllers
{
    public class AccessController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public AccessController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(VMRegister registerModel)
        {
            if (registerModel != null)
            {
                string ubicacion = registerModel.DepartamentoCentroDeFormacion + " " + registerModel.MunicipioCentroDeFormacion;

                IUsuario temporaryUser = new Usuario(registerModel.NumeroDeDocumento, registerModel.TipoDeDocumento,
                                              registerModel.Nombres,
                                              registerModel.PrimerApellido,
                                              registerModel.SegundoApellido,
                                              registerModel.Correo,
                                              registerModel.Contrasena, ubicacion, registerModel.MunicipioCentroDeFormacion,
                                              registerModel.FichaAprendiz, registerModel.IdRol);

                try
                {
                    // Intentar crear el usuario
                    await _usuarioService.CreateUsuario(temporaryUser);

                    // Si el usuario se crea exitosamente, realizar la autenticación y redireccionar
                    List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, temporaryUser.Correo),
                new Claim(ClaimTypes.Role, "Visitante")
            };

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    AuthenticationProperties properties = new AuthenticationProperties()
                    {
                        AllowRefresh = true,
                        IsPersistent = true
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), properties);

                    return RedirectToAction("Index", "Home");
                }
                catch (InvalidOperationException ex)
                {
                    // Capturar la excepción de usuario existente y devolver un mensaje de error
                    ViewData["ValidateMessage"] = ex.Message;
                    return View();
                }
            }

            ViewData["ValidateMessage"] = "Usuario inválido";
            return View();
        }





        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(VMLogin loginModel)
        {
            try
            {


                IUsuario? user = null;

                if (loginModel.Email != null && loginModel.Password != null)
                {
                    user = await _usuarioService.ValidateLogin(loginModel.Email, loginModel.Password);
                }

                if (user != null)
                {
                    // Agrega el rol y el UsuarioId del usuario a las reclamaciones
                   var claims = new List<Claim>()
                  {
                   new Claim(ClaimTypes.NameIdentifier, user.Correo),
                   new Claim(ClaimTypes.Name, user.Nombres),
                    new Claim(ClaimTypes.Surname, user.PrimerApellido),
                   new Claim(ClaimTypes.Role, ((Usuario)user).Rol.NombreRol),
                   new Claim("UsuarioId", user.UsuarioId.ToString()),
                   };

                

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var properties = new AuthenticationProperties()
                    {
                        AllowRefresh = true,
                        IsPersistent = loginModel.KeepLoggedIn
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), properties);

                    // Redirige al "Index" del "HomeController" y pasa el rol del usuario y el UsuarioId como parámetros
                    return RedirectToAction("Index", "Home", new { role = ((Usuario)user).Rol.NombreRol, userId = user.UsuarioId, userNombre = user.Nombres });
                }

                ViewData["ValidateMessage"] = "Invalid User";
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Se produjo un error en el método Login: {ex.Message}");
                // Puedes personalizar el mensaje de error que deseas mostrar al usuario en caso de que ocurra una excepción
                ViewData["ErrorMessage"] = "Se produjo un error al procesar la solicitud. Por favor, inténtelo de nuevo más tarde.";
                return View();
            }
        }


        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(string Email, string Password)
        {
            IUsuario? user = await _usuarioService.GetUsuarioByEmail(Email);

            if (user != null)
            {
                user.Contrasena = Password;

                await _usuarioService.ResetPassword(user);
            }

            ViewData["PasswordReset"] = "Si tu cuenta existe, has recuperado la contrasena!";

            return View();
        }




    }
}
