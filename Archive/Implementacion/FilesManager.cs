using Archive.Interfaz;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Archive.Implementacion
{
    public class FilesManager : IFilesManager
    {
        //private readonly string _targetFolderPath;
        public string TargetFolderPath { get; private set; }
        public FilesManager(string targetFolderPath)
        {
            TargetFolderPath = targetFolderPath;
        }

        public async Task<string> SaveFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("El archivo no puede ser nulo o vacío.");

            var fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(TargetFolderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }
        public async Task<string?> SaveAndExtractFolderAsync(IFormFile archivoZip, string targetFolderPath)
        {
            if (archivoZip == null || archivoZip.Length == 0)
                throw new ArgumentException("El archivo ZIP no puede ser nulo o vacío.");

            var uniqueFolderName = Guid.NewGuid().ToString();
            var folderPath = Path.Combine(targetFolderPath, uniqueFolderName);

            try
            {
                Directory.CreateDirectory(folderPath);


                var zipFilePath = Path.Combine(folderPath, "temp.zip");
                using (var stream = new FileStream(zipFilePath, FileMode.Create))
                {
                    await archivoZip.CopyToAsync(stream);
                }

                ZipFile.ExtractToDirectory(zipFilePath, folderPath);
                File.Delete(zipFilePath);

                return folderPath; // Devuelve la ruta de la carpeta descomprimida
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar y descomprimir archivo ZIP: {ex.Message}");
                return null;
            }
        }

        public async Task DeleteFileIfExistsAsync(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return; // No hacer nada si el nombre del archivo está vacío o nulo

            var filePath = Path.Combine(TargetFolderPath, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private bool IsZipFile(IFormFile file)
        {
            // Verificar si el archivo tiene extensión .zip
            return Path.GetExtension(file.FileName).Equals(".zip", StringComparison.OrdinalIgnoreCase);
        }
    }
}
