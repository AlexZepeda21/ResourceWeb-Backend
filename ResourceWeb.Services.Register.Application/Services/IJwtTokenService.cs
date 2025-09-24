using ResourceWeb.Services.Register.Domain.Entities;
using System;

namespace ResourceWeb.Services.Register.Application.Services
{
    public interface IJwtTokenService
    {
        TokenResult GenerateToken(UserEntity user);
    }

    public class TokenResult
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}