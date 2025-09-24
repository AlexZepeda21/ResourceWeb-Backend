using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceWeb.Services.Register.Application.DTOs
{
    public class ProfileImageResponseDto
    {
        public bool Success { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImagePublicId { get; set; }
        public string? ImageMime { get; set; }
        public string? ErrorMessage { get; set; }
        public long? SizeBytes { get; set; }
    }
}
