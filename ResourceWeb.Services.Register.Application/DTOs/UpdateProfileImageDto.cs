using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceWeb.Services.Register.Application.DTOs
{
    public class UpdateProfileImageDto
    {
        [Required(ErrorMessage = "La imagen es requerida")]
        public string ImageBase64 { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "El nombre del archivo no puede exceder 100 caracteres")]
        public string? FileName { get; set; }
    }
}
