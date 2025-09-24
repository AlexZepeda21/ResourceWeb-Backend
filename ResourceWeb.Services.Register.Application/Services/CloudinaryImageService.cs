using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceWeb.Services.Register.Application.Services
{
    public class CloudinaryImageService : IImageUploadService
    {
        private readonly Cloudinary _cloudinary;
        private readonly ILogger<CloudinaryImageService> _logger;
        private readonly string _defaultFolder;

        public CloudinaryImageService(IConfiguration configuration, ILogger<CloudinaryImageService> logger)
        {
            _logger = logger;
            _defaultFolder = "user-profiles"; // Carpeta por defecto

            // Configuración de Cloudinary
            var cloudName = configuration["Cloudinary:CloudName"];
            var apiKey = configuration["Cloudinary:ApiKey"];
            var apiSecret = configuration["Cloudinary:ApiSecret"];

            if (string.IsNullOrEmpty(cloudName) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))
            {
                throw new InvalidOperationException("Cloudinary configuration is missing. Please check CloudName, ApiKey, and ApiSecret in appsettings.json");
            }

            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<ImageUploadResult> UploadImageAsync(string imageBase64, string? fileName = null, string? folder = null)
        {
            try
            {
                _logger.LogInformation("Starting image upload to Cloudinary");

                // Validar que el base64 no esté vacío
                if (string.IsNullOrWhiteSpace(imageBase64))
                {
                    return ImageUploadResult.CreateError("Image data is required");
                }

                // Limpiar el prefijo data:image/... si existe
                var base64Data = imageBase64;
                if (base64Data.Contains(","))
                {
                    base64Data = base64Data.Split(',')[1];
                }

                // Validar formato base64
                try
                {
                    Convert.FromBase64String(base64Data);
                }
                catch (FormatException)
                {
                    return ImageUploadResult.CreateError("Invalid base64 format");
                }

                // Configurar parámetros de subida
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription($"data:image/png;base64,{base64Data}"),
                    Folder = folder ?? _defaultFolder,
                    PublicId = fileName ?? Guid.NewGuid().ToString("N"),
                    Transformation = new Transformation()
                        .Quality("auto") // Calidad automática
                        .FetchFormat("auto") // Formato automático (WebP si es compatible)
                        .Width(800).Height(800).Crop("limit") // Limitar tamaño máximo
                };

                // Subir imagen
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    _logger.LogInformation("Image uploaded successfully. PublicId: {PublicId}", uploadResult.PublicId);

                    return ImageUploadResult.CreateSuccess(
                        imageUrl: uploadResult.SecureUrl.ToString(),
                        publicId: uploadResult.PublicId,
                        mimeType: uploadResult.Format,
                        sizeBytes: uploadResult.Bytes
                    );
                }
                else
                {
                    _logger.LogError("Cloudinary upload failed. Status: {Status}, Error: {Error}",
                        uploadResult.StatusCode, uploadResult.Error?.Message);

                    return ImageUploadResult.CreateError(uploadResult.Error?.Message ?? "Upload failed");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred during image upload");
                return ImageUploadResult.CreateError($"Upload failed: {ex.Message}");
            }
        }

        public async Task<bool> DeleteImageAsync(string publicId)
        {
            try
            {
                _logger.LogInformation("Deleting image from Cloudinary. PublicId: {PublicId}", publicId);

                if (string.IsNullOrWhiteSpace(publicId))
                {
                    _logger.LogWarning("PublicId is null or empty, cannot delete image");
                    return false;
                }

                var deleteParams = new DeletionParams(publicId);
                var result = await _cloudinary.DestroyAsync(deleteParams);

                var success = result.Result == "ok";

                if (success)
                {
                    _logger.LogInformation("Image deleted successfully. PublicId: {PublicId}", publicId);
                }
                else
                {
                    _logger.LogWarning("Failed to delete image. PublicId: {PublicId}, Result: {Result}",
                        publicId, result.Result);
                }

                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while deleting image. PublicId: {PublicId}", publicId);
                return false;
            }
        }

        public string GetOptimizedImageUrl(string publicId, int? width = null, int? height = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(publicId))
                {
                    return string.Empty;
                }

                var transformation = new Transformation()
                    .Quality("auto")
                    .FetchFormat("auto");

                if (width.HasValue && height.HasValue)
                {
                    transformation = transformation.Width(width.Value).Height(height.Value).Crop("fill");
                }
                else if (width.HasValue)
                {
                    transformation = transformation.Width(width.Value);
                }
                else if (height.HasValue)
                {
                    transformation = transformation.Height(height.Value);
                }

                return _cloudinary.Api.UrlImgUp.Transform(transformation).BuildUrl(publicId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating optimized URL for PublicId: {PublicId}", publicId);
                return string.Empty;
            }
        }
    }
}