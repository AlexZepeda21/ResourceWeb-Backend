using ResourceWeb.Services.Register.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace ResourceWeb.Services.Register.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<UserEntity> GetByIdAsync(Guid Id);
        Task<UserEntity> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> UsernameExistsAsync(string username);
        Task AddAsync(UserEntity user);
        Task UpdateAsync(UserEntity user);

        // Nuevos métodos para manejar perfiles completos
        Task<UserEntity> GetByIdWithRoleAsync(Guid id);
        Task<bool> UsernameExistsExcludingUserAsync(string username, Guid excludeUserId);
    }
}
