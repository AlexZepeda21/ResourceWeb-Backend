using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceWeb.Services.Register.Application.DTOs
{
    public class ImageValidationDto
    {
        public bool IsValid { get; set; }
        public string? ErrorMessage { get; set; }
        public string? MimeType { get; set; }
        public long? SizeBytes { get; set; }

        public static ImageValidationDto CreateValid(string mimeType, long sizeBytes)
        {
            return new ImageValidationDto
            {
                IsValid = true,
                MimeType = mimeType,
                SizeBytes = sizeBytes
            };
        }

        public static ImageValidationDto CreateInvalid(string errorMessage)
        {
            return new ImageValidationDto
            {
                IsValid = false,
                ErrorMessage = errorMessage
            };
        }
    }
}
