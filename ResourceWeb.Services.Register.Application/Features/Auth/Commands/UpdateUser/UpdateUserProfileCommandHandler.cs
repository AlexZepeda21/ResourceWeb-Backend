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
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
                throw new ArgumentException("Usuario no encontrado");

            if (user.UserName != request.UserName && await _userRepository.UsernameExistsAsync(request.UserName))
                throw new ArgumentException("El nombre de usuario ya esta en uso");

            var userType = typeof(ResourceWeb.Services.Register.Domain.Entities.UserEntity);

            userType.GetProperty("UserName").SetValue(user, request.UserName);
            userType.GetProperty("Birthdate").SetValue(user, request.Birthdate);
            user.SetUpdatedAt();

            await _appDbContext.SaveChangesAsync(cancellationToken);

            return new UserProfileDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                Birthdate = user.Birthdate,
                IsActive = user.IsActive,
                Role = user.Role?.Name,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }
    }
}
