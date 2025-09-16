using MediatR;
using ResourceWeb.Services.Register.Application.DTOs;


namespace ResourceWeb.Services.Register.Application.Features.Auth.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest<UserResponseDto>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
