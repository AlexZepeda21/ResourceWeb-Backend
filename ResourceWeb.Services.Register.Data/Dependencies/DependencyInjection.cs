using Microsoft.Extensions.DependencyInjection;
using ResourceWeb.Services.Register.Data.Repositories;
using ResourceWeb.Services.Register.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceWeb.Services.Register.Data.Dependencies
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataDependencies(this IServiceCollection services)
        {
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
