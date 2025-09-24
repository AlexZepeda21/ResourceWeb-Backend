using MediatR;
using Microsoft.Extensions.Logging;
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
    public class RemoveProfileImageCommandHandler : IRequestHandler<RemoveProfileImageCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IImageUploadService _imageUploadService;
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<RemoveProfileImageCommandHandler> _logger;

        public RemoveProfileImageCommandHandler(
            IUserRepository userRepository,
            IImageUploadService imageUploadService,
            AppDbContext appDbContext,
            ILogger<RemoveProfileImageCommandHandler> logger)
        {
            _userRepository = userRepository;
            _imageUploadService = imageUploadService;
            _appDbContext = appDbContext;
            _logger = logger;
        }

        public async Task<bool> Handle(RemoveProfileImageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Processing profile image removal for user {UserId}", request.UserId);

                var user = await _userRepository.GetByIdAsync(request.UserId);
                if (user == null)
                {
                    _logger.LogWarning("User {UserId} not found", request.UserId);
                    return false;
                }

                if (string.IsNullOrEmpty(user.ImagePublicId))
                {
                    _logger.LogInformation("User {UserId} has no profile image to remove", request.UserId);
                    return true;
                }

                // Eliminar imagen de Cloudinary
                var deleteSuccess = await _imageUploadService.DeleteImageAsync(user.ImagePublicId);
                if (!deleteSuccess)
                {
                    _logger.LogWarning("Failed to delete image from Cloudinary for user {UserId}", request.UserId);
                }

                // Limpiar referencias en la entidad
                user.RemoveProfileImage();

                // Guardar cambios
                await _userRepository.UpdateAsync(user);
                await _appDbContext.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Profile image removed successfully for user {UserId}", request.UserId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while removing profile image for user {UserId}", request.UserId);
                return false;
            }
        }
    }
}
