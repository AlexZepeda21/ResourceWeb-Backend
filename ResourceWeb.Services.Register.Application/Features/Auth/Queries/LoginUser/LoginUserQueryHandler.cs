using MediatR;
using ResourceWeb.Services.Register.Application.DTOs;
using ResourceWeb.Services.Register.Application.Services;
using ResourceWeb.Services.Register.Domain.Interfaces;
using ResourceWeb.Services.Register.Domain.Interfaces.ResourceWeb.Services.Register.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceWeb.Services.Register.Application.Features.Auth.Queries.LoginUser
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, LoginResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;

        public LoginUserQueryHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<LoginResponseDto> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            // Buscar usuario por email
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
                throw new Exception("Credenciales inválidas");

            if (!user.IsActive)
                throw new Exception("Usuario inactivo");

            // Verificar contraseña
            var isPasswordValid = _passwordHasher.VerifyPassword(request.Password, user.PasswordHash);
            if (!isPasswordValid)
                throw new Exception("Credenciales inválidas");

            var tokenResult = _jwtTokenService.GenerateToken(user);

            var userDto = new UserResponseDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Birthdate = user.Birthdate,
                ImageUrl = user.ImageUrl, 
                Gender = user.Gender,
                Language = user.Language,
                RoleName = user.Role?.Name ?? "User", 
                EmailConfirmed = user.EmailConfirmed,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };

            return new LoginResponseDto
            {
                Token = tokenResult.Token,
                ExpiresAt = tokenResult.ExpiresAt,
                User = userDto
            };
        }
    }
}