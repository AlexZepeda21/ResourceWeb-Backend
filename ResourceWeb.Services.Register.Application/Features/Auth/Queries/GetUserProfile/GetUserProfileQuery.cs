using MediatR;
using ResourceWeb.Services.Register.Application.DTOs;
using System;

namespace ResourceWeb.Services.Register.Application.Features.Users.Queries.GetUserProfile
{
    public class GetUserProfileQuery : IRequest<UserProfileDto>
    {
        public Guid UserId { get; set; }

        public GetUserProfileQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}