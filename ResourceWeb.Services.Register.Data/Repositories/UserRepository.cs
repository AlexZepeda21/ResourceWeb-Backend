using Microsoft.EntityFrameworkCore;
using ResourceWeb.Services.Register.Data.Context;
using ResourceWeb.Services.Register.Domain.Entities;
using ResourceWeb.Services.Register.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace ResourceWeb.Services.Register.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserEntity> GetByIdAsync(Guid id)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<UserEntity> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Email == email);
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.Users
                .AnyAsync(u => u.UserName == username);
        }

        public async Task AddAsync(UserEntity user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task UpdateAsync(UserEntity user)
        {
            _context.Users.Update(user);
            await Task.CompletedTask;
        }
    }
}