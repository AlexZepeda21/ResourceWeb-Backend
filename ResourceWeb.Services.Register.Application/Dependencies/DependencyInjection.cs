using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ResourceWeb.Services.Register.Application.Services;
using ResourceWeb.Services.Register.Domain.Interfaces.ResourceWeb.Services.Register.Domain.Interfaces;
using System.Reflection;


namespace ResourceWeb.Services.Register.Application.Dependencies
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            // Registra el password hasher
            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
