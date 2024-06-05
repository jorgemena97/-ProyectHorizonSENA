using Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;

public class FileModel
{
    public int PageNumber { get; set; }
    public int PageSize {  get; set; }
    public int TotalItems { get; set; }
   public int IdUsuario { get; set; }
   public bool IsRootProjects { set; get; }
    public string Name { get; set; }
    public string Path { get; set; } // Ruta absoluta del archivo en el sistema de archivos del servidor
    public bool IsDirectory { get; set; }
    public List<Comentarios> Comentarios { get; set; }
    public int IdProyecto { get; set; }
    public List<FileModel> Children { get; set; } = new List<FileModel>();

    // Propiedad para la ruta relativa del archivo dentro del contexto de la aplicación web
    public string RelativePath
    {
        get
        {
            if (string.IsNullOrEmpty(this.Path))
            {
                return string.Empty; // o algún valor por defecto
            }

            // Obtener la ruta del directorio raíz de la aplicación web (wwwroot)
            string webRootPath = Directory.GetCurrentDirectory(); // Obtener la ruta del directorio actual
            string wwwrootPath = webRootPath + "/wwwroot"; // Combinar con la carpeta "wwwroot" usando concatenación de cadenas

            // Reemplazar las barras invertidas (\) con barras normales (/) en la ruta del archivo
            var normalizedFilePath = this.Path.Replace("\\", "/");

            // Eliminar la parte de la ruta del directorio raíz de la aplicación web para obtener la ruta relativa
            var relativePath = normalizedFilePath.Replace(wwwrootPath, "").TrimStart('/');

            return relativePath;
        }
    }

    public string UsuarioNombre { get; set; }
    public string UsuarioApellido { get; set; }
    public string NombreProyecto { get; set; }
}
