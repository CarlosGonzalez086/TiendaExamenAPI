using System.Text.RegularExpressions;
using TiendaExamenAPI.DbData.DtoModels.Response;

namespace TiendaExamenAPI.Services.FuncionesGenerales
{
    public class ImageStorageService
    {
        private readonly string _imagesFolderPath;
        private readonly string _imagesRelativeUrl = "/images/";

        private const long MaxFileBytes = 5 * 1024 * 1024;
        private static readonly string[] AllowedExt = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        public ImageStorageService(string webRootPath = null)
        {
            var webRoot = string.IsNullOrEmpty(webRootPath)
                ? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")
                : webRootPath;

            _imagesFolderPath = Path.Combine(webRoot, "images");
            if (!Directory.Exists(_imagesFolderPath))
            {
                Directory.CreateDirectory(_imagesFolderPath);
            }
        }



        public async Task<Response> SaveImageFromFormFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return new Response { codigo = "901", mensaje = "El campo de la imagen es requerido", respuesta = "" };
            }

            if (file.Length > MaxFileBytes)
            {
                return new Response { codigo = "903", mensaje = "La imagen supera el tamaño permitido", respuesta = "" };
            }

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExt.Contains(ext))
            {
                return new Response { codigo = "902", mensaje = "Tipo de imagen no permitido", respuesta = "" };
            }

            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(_imagesFolderPath, fileName);

            try
            {
                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                }

                var relativeUrl = $"{_imagesRelativeUrl}{fileName}";
                return new Response { codigo = "000", mensaje = "Imagen guardada", respuesta = relativeUrl };
            }
            catch (Exception ex)
            {
                return new Response { codigo = "905", mensaje = "Error al guardar la imagen", respuesta = ex.Message };
            }
        }

        public Response SaveImageFromBase64(string base64)
        {
            if (string.IsNullOrWhiteSpace(base64))
            {
                return new Response { codigo = "901", mensaje = "El campo de la imagen es requerido", respuesta = "" };
            }

            var match = Regex.Match(base64, @"data:(?<type>.+?);base64,(?<data>.+)");
            if (match.Success)
            {
                base64 = match.Groups["data"].Value;
            }

            byte[] bytes;
            try
            {
                bytes = Convert.FromBase64String(base64);
            }
            catch
            {
                return new Response { codigo = "904", mensaje = "Imagen en base64 inválida", respuesta = "" };
            }

            if (bytes.Length > MaxFileBytes)
            {
                return new Response { codigo = "903", mensaje = "La imagen supera el tamaño permitido", respuesta = "" };
            }

            string ext = GetExtensionFromBytes(bytes);
            if (!AllowedExt.Contains(ext))
            {
                return new Response { codigo = "902", mensaje = "Tipo de imagen no permitido", respuesta = "" };
            }

            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(_imagesFolderPath, fileName);

            try
            {
                System.IO.File.WriteAllBytes(filePath, bytes);
                var relativeUrl = $"{_imagesRelativeUrl}{fileName}";
                return new Response { codigo = "000", mensaje = "Imagen guardada", respuesta = relativeUrl };
            }
            catch (Exception ex)
            {
                return new Response { codigo = "905", mensaje = "Error al guardar la imagen", respuesta = ex.Message };
            }
        }

        public async Task<Response> SaveImageAsync(IFormFile file, string imagenBase64)
        {
            if (file != null)
            {
                return await SaveImageFromFormFileAsync(file);
            }

            return SaveImageFromBase64(imagenBase64);
        }

        private string GetExtensionFromBytes(byte[] bytes)
        {
            if (bytes.Length >= 4 && bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47)
                return ".png"; // PNG
            if (bytes.Length >= 3 && bytes[0] == 0xFF && bytes[1] == 0xD8 && bytes[2] == 0xFF)
                return ".jpg"; // JPG/JPEG
            if (bytes.Length >= 4 && bytes[0] == 0x47 && bytes[1] == 0x49 && bytes[2] == 0x46)
                return ".gif"; // GIF
            if (bytes.Length >= 12 && bytes[8] == 0x57 && bytes[9] == 0x45 && bytes[10] == 0x42 && bytes[11] == 0x50)
                return ".webp"; // WEBP
            return ".jpg"; 
        }
    }
}
