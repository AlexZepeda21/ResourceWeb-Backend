using MediatR;
using Microsoft.EntityFrameworkCore;
using ResourceWeb.Services.Register.Application.DTOs;
using ResourceWeb.Services.Register.Data.Context;
using ResourceWeb.Services.Register.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceWeb.Services.Register.Application.Features.Auth.Commands.UpdateUser
{
    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, UserProfileDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly AppDbContext _appDbContext;

        public UpdateUserProfileCommandHandler(
            IUserRepository userRepository,
            AppDbContext context)
        {
            _userRepository = userRepository;
            _appDbContext = context;
        }

        public async Task<UserProfileDto> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            // 1. Obtener usuario con rol
            var user = await _userRepository.GetByIdWithRoleAsync(request.UserId);
            if (user == null)
                throw new ArgumentException("Usuario no encontrado");

            // 2. Verificar username único (si cambió)
            if (user.UserName != request.UserName && await _userRepository.UsernameExistsExcludingUserAsync(request.UserName, request.UserId))
                throw new ArgumentException("El nombre de usuario ya está en uso");

            // 3. Actualizar campos usando reflection (manteniendo tu approach)
            var userType = typeof(ResourceWeb.Services.Register.Domain.Entities.UserEntity);

            // Campos básicos
            userType.GetProperty("UserName")?.SetValue(user, request.UserName);
            userType.GetProperty("Birthdate")?.SetValue(user, request.Birthdate);

            // Nuevos campos
            userType.GetProperty("Gender")?.SetValue(user, request.Gender);
            userType.GetProperty("Language")?.SetValue(user, request.Language);
            userType.GetProperty("ModeUi")?.SetValue(user, request.ModeUi);

            // Actualizar timestamp
            user.SetUpdatedAt();

            // 4. Guardar cambios
            await _userRepository.UpdateAsync(user);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            // 5. Retornar perfil completo actualizado
            return new UserProfileDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                IsActive = user.IsActive,

                // Datos de imagen
                ImageUrl = user.ImageUrl,
                ImagePublicId = user.ImagePublicId,
                ImageMime = user.ImageMime,

                // Información del perfil
                Birthdate = user.Birthdate,
                Gender = user.Gender,
                Language = user.Language,
                ModeUi = user.ModeUi,

                // Rol
                RoleName = user.Role?.Name,

                // Metadata
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }
    }
}
