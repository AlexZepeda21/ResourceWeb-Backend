using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceWeb.Services.Register.Application.Services
{
    public interface IImageUploadService
    {
        /// <summary>
        /// Sube una imagen a Cloudinary desde base64
        /// </summary>
        /// <param name="imageBase64">Imagen en formato base64</param>
        /// <param name="fileName">Nombre del archivo (opcional)</param>
        /// <param name="folder">Carpeta donde guardar (opcional)</param>
        /// <returns>Resultado con URL y PublicId</returns>
        Task<ImageUploadResult> UploadImageAsync(string imageBase64, string? fileName = null, string? folder = null);

        /// <summary>
        /// Elimina una imagen de Cloudinary
        /// </summary>
        /// <param name="publicId">PublicId de la imagen en Cloudinary</param>
        /// <returns>True si se eliminó correctamente</returns>
        Task<bool> DeleteImageAsync(string publicId);

        /// <summary>
        /// Obtiene la URL optimizada de una imagen
        /// </summary>
        /// <param name="publicId">PublicId de la imagen</param>
        /// <param name="width">Ancho deseado (opcional)</param>
        /// <param name="height">Alto deseado (opcional)</param>
        /// <returns>URL optimizada</returns>
        string GetOptimizedImageUrl(string publicId, int? width = null, int? height = null);
    }

    public class ImageUploadResult
    {
        public bool Success { get; set; }
        public string? ImageUrl { get; set; }
        public string? PublicId { get; set; }
        public string? MimeType { get; set; }
        public string? ErrorMessage { get; set; }
        public long? SizeBytes { get; set; }

        public static ImageUploadResult CreateSuccess(string imageUrl, string publicId, string mimeType, long sizeBytes)
        {
            return new ImageUploadResult
            {
                Success = true,
                ImageUrl = imageUrl,
                PublicId = publicId,
                MimeType = mimeType,
                SizeBytes = sizeBytes
            };
        }

        public static ImageUploadResult CreateError(string errorMessage)
        {
            return new ImageUploadResult
            {
                Success = false,
                ErrorMessage = errorMessage
            };
        }
    }
}