using Microsoft.EntityFrameworkCore;
using ResourceWeb.Services.Register.Application.Dependencies;
using ResourceWeb.Services.Register.Data.Context;
using ResourceWeb.Services.Register.Data.Dependencies;
using ResourceWeb.Services.Register.Data.Seeders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStringMz")));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(ResourceWeb.Services.Register.Application.Dependencies.DependencyInjection).Assembly));

builder.Services.AddDataDependencies();
builder.Services.AddApplicationDependencies();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Ejecutar seeders al iniciar la aplicación
using (var scope = app.Services.CreateScope())
{
    await SeedDatabaseAsync(scope.ServiceProvider);
}

app.Run();

// Función local (sin modifier 'public')
static async Task SeedDatabaseAsync(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Aplicar migraciones y crear la base de datos
    await context.Database.MigrateAsync();

    // Ejecutar seeders después de crear/actualizar la BD
    await RoleSeeder.SeedAsync(context);
}