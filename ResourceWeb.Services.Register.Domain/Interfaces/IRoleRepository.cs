using ResourceWeb.Services.Register.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace ResourceWeb.Services.Register.Domain.Interfaces
{
    public interface IRoleRepository
    {
        Task<RoleEntity> GetByIdAsync(Guid id);
        Task<RoleEntity> GetByNameAsync(string name);
        Task<bool> RoleExistsAsync(string name);
        Task AddAsync(RoleEntity role);
        Task UpdateAsync(RoleEntity role);
    }
}