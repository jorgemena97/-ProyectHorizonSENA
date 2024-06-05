using System.IO;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Interfaces;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;

public class FileController : Controller
{
    private readonly IProyectoService _proyectoService;

    public FileController(IProyectoService proyectoService)
    {
        _proyectoService = proyectoService;
    }

    public async Task<IActionResult> Index()
    {
        var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/proyectos");
        var rootDirectory = await GetDirectoryStructure(rootPath);
        rootDirectory.IsRootProjects = true; // Establecer que es la carpeta raíz "proyectos"
        return View(rootDirectory);
    }


    private async Task<FileModel> GetDirectoryStructure(string path, bool isRootProjects = false)
    {
        var dirInfo = new DirectoryInfo(path);
        var fileModel = new FileModel
        {
            Name = dirInfo.Name,
            Path = dirInfo.FullName,
            IsDirectory = true,
            IsRootProjects = isRootProjects // Esto se establecerá como verdadero para la carpeta raíz "proyectos"
        };

        // Obtener la primera subcarpeta si existe
        var firstSubDirectory = dirInfo.GetDirectories().FirstOrDefault();
        if (firstSubDirectory != null)
        {
            fileModel.Name = firstSubDirectory.Name;
        }
        var proyecto = await _proyectoService.GetByPathAsync(dirInfo.FullName);
        if (proyecto != null)
        {
            fileModel.UsuarioNombre = proyecto.Nombres;
            fileModel.UsuarioApellido = proyecto.PrimerApellido;
            fileModel.NombreProyecto = proyecto.NombreProyecto;
            fileModel.IdUsuario = proyecto.IdUsuario ?? 0; // Usa ?? para asignar 0 si IdUsuario es null
        }

        foreach (var directory in dirInfo.GetDirectories())
        {
            var childModel = await GetDirectoryStructure(directory.FullName, directory.Name.Equals("Proyectos", StringComparison.OrdinalIgnoreCase));
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
                fileChildModel.UsuarioNombre = proyecto.Nombres;
                fileChildModel.UsuarioApellido = proyecto.PrimerApellido;
                fileChildModel.NombreProyecto = proyecto.NombreProyecto;
                fileChildModel.IdUsuario = proyecto.IdUsuario ?? 0; // Usa ?? para asignar 0 si IdUsuario es null
            }
            fileModel.Children.Add(fileChildModel);
        }

        return fileModel;
    }

    public IActionResult Download(string filePath)
    {
        if (System.IO.File.Exists(filePath))
        {
            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(filePath), Path.GetFileName(filePath));
        }
        return NotFound();
    }

    [HttpGet]
    public IActionResult ViewFileContent(string filePath)
    {
        var absoluteFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath);

        if (System.IO.File.Exists(absoluteFilePath))
        {
            if (filePath.EndsWith(".doc") || filePath.EndsWith(".docx"))
            {
                var content = ConvertWordToHtml(absoluteFilePath);
                return Content(content, "text/html");
            }
            else if (filePath.EndsWith(".png") || filePath.EndsWith(".jpg") || filePath.EndsWith(".jpeg") || filePath.EndsWith(".gif"))
            {
                var bytes = System.IO.File.ReadAllBytes(absoluteFilePath);
                return File(bytes, GetContentType(filePath));
            }
            else
            {
                var content = System.IO.File.ReadAllText(absoluteFilePath);
                return Content(content, "text/plain");
            }
        }

        return NotFound();
    }

    private string ConvertWordToHtml(string filePath)
    {
        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, false))
        {
            var mainPart = wordDoc.MainDocumentPart;
            var htmlContent = new StringBuilder();

            foreach (var element in mainPart.Document.Body.Elements())
            {
                ConvertToHtml(element, htmlContent, mainPart);
            }

            return htmlContent.ToString();
        }
    }

    private void ConvertToHtml(OpenXmlElement element, StringBuilder htmlContent, MainDocumentPart mainPart)
    {
        switch (element)
        {
            case Paragraph paragraph:
                htmlContent.Append("<p>");
                foreach (var run in paragraph.Elements<Run>())
                {
                    foreach (var text in run.Elements<Text>())
                    {
                        htmlContent.Append(text.Text);
                    }

                    foreach (var picture in run.Elements<Drawing>())
                    {
                        var image = picture.Descendants<DocumentFormat.OpenXml.Drawing.Blip>().FirstOrDefault();
                        if (image != null)
                        {
                            var imageId = image.Embed.Value;
                            var imagePart = (ImagePart)mainPart.GetPartById(imageId);
                            var imageBytes = GetImageBytes(imagePart);

                            var base64 = Convert.ToBase64String(imageBytes);
                            var mimeType = imagePart.ContentType;
                            htmlContent.Append($"<img src='data:{mimeType};base64,{base64}' style='max-width:200px; max-height:200px;' />");
                        }
                    }
                }
                htmlContent.Append("</p>");
                break;

            case Table table:
                htmlContent.Append("<table border='1'>");
                foreach (var row in table.Elements<TableRow>())
                {
                    htmlContent.Append("<tr>");
                    foreach (var cell in row.Elements<TableCell>())
                    {
                        htmlContent.Append("<td>");
                        foreach (var cellElement in cell.Elements())
                        {
                            ConvertToHtml(cellElement, htmlContent, mainPart);
                        }
                        htmlContent.Append("</td>");
                    }
                    htmlContent.Append("</tr>");
                }
                htmlContent.Append("</table>");
                break;

            // Add more cases to handle other elements like headers, footers, etc.
            default:
                break;
        }
    }

    private byte[] GetImageBytes(ImagePart imagePart)
    {
        using (var stream = imagePart.GetStream())
        using (var memoryStream = new MemoryStream())
        {
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }

    private string GetContentType(string path)
    {
        var types = new Dictionary<string, string>
        {
            { ".txt", "text/plain" },
            { ".pdf", "application/pdf" },
            { ".doc", "application/msword" },
            { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
            { ".xls", "application/vnd.ms-excel" },
            { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
            { ".png", "image/png" },
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".gif", "image/gif" },
            { ".csv", "text/csv" }
        };

        var ext = Path.GetExtension(path).ToLowerInvariant();
        return types.ContainsKey(ext) ? types[ext] : "application/octet-stream";
    }
}
