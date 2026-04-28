using Microsoft.EntityFrameworkCore;
using ProjetoTaskManager.Application.Interfaces;
using ProjetoTaskManager.Domain.Entities;
using ProjetoTaskManager.Infrastructure.Persistence;

namespace ProjetoTaskManager.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<User?> GetByEmailAsync(string email) =>
            await _context.users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<bool> EmailExistsAsync(string email) =>
            await _context.users.AnyAsync(u => u.Email == email);
    }
}