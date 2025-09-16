using FluentValidation;
using ResourceWeb.Services.Register.Application.Features.Auth.Commands.RegisterUser;

namespace ResourceWeb.Services.Register.Application.Validators
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("El nombre de usuario es requerido")
                .MaximumLength(50).WithMessage("El nombre de usuario no puede exceder 50 caracteres");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El correo electrónico es requerido")
                .EmailAddress().WithMessage("Formato de correo electrónico inválido")
                .MaximumLength(100).WithMessage("El correo electrónico no puede exceder 100 caracteres");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es requerida")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres");
        }
    }
}