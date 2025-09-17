using ResourceWeb.Services.Register.Domain.Entities;
using System;

namespace ResourceWeb.Services.Register.Application.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(UserEntity user);
    }
}