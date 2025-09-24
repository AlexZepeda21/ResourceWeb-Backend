using ResourceWeb.Services.Register.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceWeb.Services.Register.Domain.Entities
{
    public class UserEntity : BaseEntity
    {
        public string UserName { get; protected set; }
        public string Email { get; protected set; }
        public bool EmailConfirmed { get; protected set; }
        public string PasswordHash { get; protected set; }
        public bool IsActive { get; protected set; } = true;

        public string? ImageUrl { get; protected set; }
        public string? ImagePublicId { get; protected set; }
        public string? ImageMime { get; protected set; }

        public DateTime? Birthdate { get; protected set; }
        public string? Gender { get; protected set; }
        public string? Language { get; protected set; } 
        public bool? ModeUi { get; protected set; }
        public Guid RoleId { get; protected set; }
        public RoleEntity Role { get; protected set; }

        protected UserEntity() { }

        public UserEntity(string userName, string email, string passwordHash, Guid roleId)
        {
            UserName = userName;
            Email = email;
            PasswordHash = passwordHash;
            RoleId = roleId;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateProfile(DateTime? birthdate, string? gender, string? language, bool? modeui)
        {
            Birthdate = birthdate;
            Gender = gender;
            Language = language;
            ModeUi = modeui;
            UpdatedAt = DateTime.UtcNow;

        }

        public void UpdateProfileImage(string imageUrl, string publicId, string mimeType)
        {
            ImageUrl = imageUrl;
            ImagePublicId = publicId;
            ImageMime = mimeType;
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveProfileImage()
        {
            ImageUrl = null;
            ImagePublicId = null;
            ImageMime = null;
            UpdatedAt = DateTime.UtcNow;
        }
        public void SetActive(bool isActive)
        {
            IsActive = isActive;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}

