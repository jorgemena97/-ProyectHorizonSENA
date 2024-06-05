using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archive.Interfaz
{
    public interface IFilesManager
    {
        Task<string> SaveFileAsync(IFormFile file);
        Task<string?> SaveAndExtractFolderAsync(IFormFile archivoZip, string targetFolderPath);
        Task DeleteFileIfExistsAsync(string fileName);
    }
}
