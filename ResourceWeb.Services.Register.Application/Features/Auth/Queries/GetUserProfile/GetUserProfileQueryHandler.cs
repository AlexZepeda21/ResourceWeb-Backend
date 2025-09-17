using MediatR;
using ResourceWeb.Services.Register.Application.DTOs;
using ResourceWeb.Services.Register.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceWeb.Services.Register.Application.Features.Users.Queries.GetUserProfile
{
    public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileDto>
    {
        private readonly IUserRepository _userRepository;

        public GetUserProfileQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserProfileDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);

            if (user == null)
                throw new Exception("Usuario no encontrado");

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