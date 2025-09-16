using MediatR;
using ResourceWeb.Services.Register.Application.DTOs;
using ResourceWeb.Services.Register.Domain.Entities;
using ResourceWeb.Services.Register.Domain.Interfaces;
using ResourceWeb.Services.Register.Domain.Interfaces.ResourceWeb.Services.Register.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceWeb.Services.Register.Application.Features.Auth.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterUserCommandHandler(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            {
                if (await _userRepository.EmailExistsAsync(request.Email))
                    throw new Exception("El email ya está registrado");

                if (await _userRepository.UsernameExistsAsync(request.UserName))
                    throw new Exception("El nombre de usuario ya existe");

                var userRole = await _roleRepository.GetByNameAsync("User");
                if (userRole == null)
                    throw new Exception("Rol por defecto no configurado");

                var passwordHash = _passwordHasher.HashPassword(request.Password);

                var user = new UserEntity(
                userName: request.UserName,
                email: request.Email,
                passwordHash: passwordHash,
                roleId: userRole.Id
                );

                await _userRepository.AddAsync(user);

                return new UserResponseDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = userRole.Name
                };
            }
        }
    }
}