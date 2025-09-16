using Microsoft.EntityFrameworkCore;
using ResourceWeb.Services.Register.Data.Context;
using ResourceWeb.Services.Register.Domain.Entities;
using ResourceWeb.Services.Register.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace ResourceWeb.Services.Register.Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RoleEntity> GetByIdAsync(Guid id)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<RoleEntity> GetByNameAsync(string name)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == name);
        }

        public async Task<bool> RoleExistsAsync(string name)
        {
            return await _context.Roles
                .AnyAsync(r => r.Name == name);
        }

        public async Task AddAsync(RoleEntity role)
        {
            await _context.Roles.AddAsync(role);
        }

        public async Task UpdateAsync(RoleEntity role)
        {
            _context.Roles.Update(role);
            await Task.CompletedTask;
        }
    }
}