using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ResourceWeb.Services.Register.Application.Behaviors;
using ResourceWeb.Services.Register.Application.Services;
using ResourceWeb.Services.Register.Domain.Interfaces.ResourceWeb.Services.Register.Domain.Interfaces;
using System.Reflection;


namespace ResourceWeb.Services.Register.Application.Dependencies
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

            services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddScoped<IJwtTokenService, JwtTokenService>();

            services.AddScoped<IImageUploadService, CloudinaryImageService>();

            return services;
        }
    }
}
