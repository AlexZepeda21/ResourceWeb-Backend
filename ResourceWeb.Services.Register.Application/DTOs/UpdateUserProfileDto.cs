using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceWeb.Services.Register.Application.DTOs
{
    public class UpdateUserProfileDto
    {
        [StringLength(50, ErrorMessage = "El nombre de usuario no puede exceder 50 caracteres")]
        public string? UserName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Birthdate { get; set; }

        [StringLength(20, ErrorMessage = "El género no puede exceder 20 caracteres")]
        [RegularExpression("^(Male|Female|Other|PreferNotToSay)$",
            ErrorMessage = "El género debe ser: Male, Female, Other o PreferNotToSay")]
        public string? Gender { get; set; }

        [StringLength(10, ErrorMessage = "El idioma no puede exceder 10 caracteres")]
        [RegularExpression("^[a-z]{2}(-[A-Z]{2})?$",
            ErrorMessage = "El idioma debe tener formato: es, en, es-MX, etc.")]
        public string? Language { get; set; }

        public bool? ModeUi { get; set; }
    }
}
