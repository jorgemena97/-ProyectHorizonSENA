using Archive;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repository.Implementation;
using Persistence.Repository.Interfaces;
using Persistence.Services.Implementation;
using Persistence.Services.Interfaces;
using Services.Services.Implementation;
using Services.Services.Interfaces;
using DinkToPdf;
using DinkToPdf.Contracts;
using PorjectHorizonSENA.Extension;
using Archive.Interfaz;
using Archive.Implementacion;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddDbContext<HorizonteDBContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
builder.Services.AddTransient<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddTransient<IUsuarioService, UsuarioService>();
builder.Services.AddTransient<IPasswordHasher, PasswordHasher>();
builder.Services.AddTransient<IProyectoRepository, ProyectoRepository>();
builder.Services.AddTransient<IProyectoService, ProyectoService>();

//Registrar FilesManager como servicio con la ruta de la carpeta de destino
var targetFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
builder.Services.AddTransient<FilesManager>(provider => new FilesManager(targetFolderPath));

var context = new CustomAssemblyLoadContext();
context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "LibreriaPDF/libwkhtmltox.dll"));
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    option.LoginPath = "/Access/Login";
    option.ExpireTimeSpan = TimeSpan.FromHours(2);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Access}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "perfil",
    pattern: "Perfil/{id:int}",
    defaults: new { controller = "Perfil", action = "Perfil" });

app.MapControllerRoute(
    name: "miPerfil",
    pattern: "Perfil/MiPerfil",
    defaults: new { controller = "Perfil", action = "MiPerfil" });

app.Run();
