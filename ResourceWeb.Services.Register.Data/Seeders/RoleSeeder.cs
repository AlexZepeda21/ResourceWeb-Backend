using Microsoft.EntityFrameworkCore;
using ResourceWeb.Services.Register.Data.Context;
using ResourceWeb.Services.Register.Domain.Entities;

namespace ResourceWeb.Services.Register.Data.Seeders
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            // Verificar si ya existen roles
            if (await context.Roles.AnyAsync())
                return;

            // Crear rol de administrador
            var adminRole = new RoleEntity("Admin", "Administrador del sistema");

            // Crear rol de usuario regular
            var userRole = new RoleEntity("User", "Usuario regular");

            // Agregar roles al contexto
            await context.Roles.AddRangeAsync(adminRole, userRole);

            // Guardar cambios
            await context.SaveChangesAsync();
        }
    }
}