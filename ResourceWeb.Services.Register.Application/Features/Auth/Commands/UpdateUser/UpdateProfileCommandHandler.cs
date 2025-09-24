using MediatR;
using Microsoft.Extensions.Logging;
using ResourceWeb.Services.Register.Application.DTOs;
using ResourceWeb.Services.Register.Application.Services;
using ResourceWeb.Services.Register.Data.Context;
using ResourceWeb.Services.Register.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceWeb.Services.Register.Application.Features.Auth.Commands.UpdateUser
{
    public class UploadProfileImageCommandHandler : IRequestHandler<UploadProfileImageCommand, ProfileImageResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IImageUploadService _imageUploadService;
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<UploadProfileImageCommandHandler> _logger;

        public UploadProfileImageCommandHandler(
            IUserRepository userRepository,
            IImageUploadService imageUploadService,
            AppDbContext appDbContext,
            ILogger<UploadProfileImageCommandHandler> logger)
        {
            _userRepository = userRepository;
            _imageUploadService = imageUploadService;
            _appDbContext = appDbContext;
            _logger = logger;
        }

        public async Task<ProfileImageResponseDto> Handle(UploadProfileImageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Processing profile image upload for user {UserId}", request.UserId);

                // 1. Buscar al usuario
                var user = await _userRepository.GetByIdAsync(request.UserId);
                if (user == null)
                {
                    return new ProfileImageResponseDto
                    {
                        Success = false,
                        ErrorMessage = "Usuario no encontrado"
                    };
                }

                // 2. Si el usuario ya tiene una imagen, eliminarla de Cloudinary
                if (!string.IsNullOrEmpty(user.ImagePublicId))
                {
                    _logger.LogInformation("Deleting existing image for user {UserId}", request.UserId);
                    await _imageUploadService.DeleteImageAsync(user.ImagePublicId);
                }

                // 3. Subir la nueva imagen a Cloudinary
                var uploadResult = await _imageUploadService.UploadImageAsync(
                    request.ImageBase64,
                    request.FileName ?? $"user_{request.UserId}",
                    "user-profiles"
                );

                if (!uploadResult.Success)
                {
                    _logger.LogError("Failed to upload image to Cloudinary: {Error}", uploadResult.ErrorMessage);
                    return new ProfileImageResponseDto
                    {
                        Success = false,
                        ErrorMessage = uploadResult.ErrorMessage
                    };
                }

                // 4. Actualizar la entidad del usuario usando el método de dominio
                user.UpdateProfileImage(
                    uploadResult.ImageUrl!,
                    uploadResult.PublicId!,
                    uploadResult.MimeType!
                );

                // 5. Guardar cambios usando repository
                await _userRepository.UpdateAsync(user);
                await _appDbContext.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Profile image updated successfully for user {UserId}", request.UserId);

                return new ProfileImageResponseDto
                {
                    Success = true,
                    ImageUrl = uploadResult.ImageUrl,
                    ImagePublicId = uploadResult.PublicId,
                    ImageMime = uploadResult.MimeType,
                    SizeBytes = uploadResult.SizeBytes
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while uploading profile image for user {UserId}", request.UserId);
                return new ProfileImageResponseDto
                {
                    Success = false,
                    ErrorMessage = "Error interno del servidor al subir la imagen"
                };
            }
        }
    }
}
