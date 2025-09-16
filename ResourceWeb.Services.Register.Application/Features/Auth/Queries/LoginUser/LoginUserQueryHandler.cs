using MediatR;
using ResourceWeb.Services.Register.Application.DTOs;
using ResourceWeb.Services.Register.Domain.Interfaces;
using ResourceWeb.Services.Register.Domain.Interfaces.ResourceWeb.Services.Register.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceWeb.Services.Register.Application.Features.Auth.Queries.LoginUser
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, LoginResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public LoginUserQueryHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<LoginResponseDto> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
                throw new Exception("Credenciales inválidas");

            if (!user.IsActive)
                throw new Exception("Usuario inactivo");

            var isPasswordValid = _passwordHasher.VerifyPassword(request.Password, user.PasswordHash);
            if (!isPasswordValid)
                throw new Exception("Credenciales inválidas");

            return new LoginResponseDto
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role?.Name ?? "User",
                Token = "JWT_TOKEN_AQUI", 
                Expiration = DateTime.UtcNow.AddHours(2)
            };
        }
    }
}