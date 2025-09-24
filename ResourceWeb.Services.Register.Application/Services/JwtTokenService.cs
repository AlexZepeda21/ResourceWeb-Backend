using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ResourceWeb.Services.Register.Application.Services;
using ResourceWeb.Services.Register.Domain.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ResourceWeb.Services.Register.Application.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TokenResult GenerateToken(UserEntity user)
        {
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]
                    ?? "MiClaveSecretaSuperSeguraQueDebeSerMuyLarga123456789"));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role?.Name ?? "User"),
                new Claim("userId", user.Id.ToString())
            };

            var expiresAt = DateTime.UtcNow.AddHours(2);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"] ?? "ResourceWebAPI",
                audience: _configuration["Jwt:Audience"] ?? "ResourceWebClient",
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new TokenResult
            {
                Token = tokenString,
                ExpiresAt = expiresAt
            };
        }
    }
}
