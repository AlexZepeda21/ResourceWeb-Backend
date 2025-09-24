using MediatR;
using ResourceWeb.Services.Register.Application.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceWeb.Services.Register.Application.Features.Auth.Commands.UpdateUser
{
    public class UploadProfileImageCommand : IRequest<ProfileImageResponseDto>
    {
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "La imagen es requerida")]
        public string ImageBase64 { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "El nombre del archivo no puede exceder 100 caracteres")]
        public string? FileName { get; set; }

        public UploadProfileImageCommand(Guid userId, string imageBase64, string? fileName = null)
        {
            UserId = userId;
            ImageBase64 = imageBase64;
            FileName = fileName;
        }

        public UploadProfileImageCommand() { }
    }
}
