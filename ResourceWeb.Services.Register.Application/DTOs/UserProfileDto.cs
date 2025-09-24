using System;

namespace ResourceWeb.Services.Register.Application.DTOs
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }
        public bool IsActive { get; set; }

        // Imagen de perfil
        public string? ImageUrl { get; set; }
        public string? ImagePublicId { get; set; }
        public string? ImageMime { get; set; }

        // Información adicional del perfil
        public DateTime? Birthdate { get; set; }
        public string? Gender { get; set; }
        public string? Language { get; set; }
        public bool? ModeUi { get; set; }

        // Información del rol
        public string RoleName { get; set; } = string.Empty;

        // Metadata
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}