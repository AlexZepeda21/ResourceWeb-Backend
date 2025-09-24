using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ResourceWeb.Services.Register.Application.Dependencies;
using ResourceWeb.Services.Register.Data.Context;
using ResourceWeb.Services.Register.Data.Dependencies;
using ResourceWeb.Services.Register.Data.Seeders;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStringMz")));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(ResourceWeb.Services.Register.Application.Dependencies.DependencyInjection).Assembly));

builder.Services.AddDataDependencies();
builder.Services.AddApplicationDependencies();

// Configuración JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "ResourceWebAPI",
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "ResourceWebClient",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"] ?? "MiClaveSecretaSuperSeguraQueDebeSerMuyLarga123456789"))
        };
    });

builder.Services.AddAuthorization();

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

// ¡IMPORTANTE! El orden es crucial
app.UseAuthentication(); // Debe ir antes de UseAuthorization
app.UseAuthorization();
app.UseHttpsRedirection();

app.UseCors("AllowAll");
app.MapControllers();

// Ejecutar seeders al iniciar la aplicación
using (var scope = app.Services.CreateScope())
{
    await SeedDatabaseAsync(scope.ServiceProvider);
}

// Configure the HTTP request pipeline.



app.Run();


static async Task SeedDatabaseAsync(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await context.Database.MigrateAsync();
    await RoleSeeder.SeedAsync(context);

}