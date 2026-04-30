using Microsoft.EntityFrameworkCore;
using ProjetoTaskManager.Application.Interfaces;
using ProjetoTaskManager.Domain.Entities;
using ProjetoTaskManager.Infrastructure.Persistence;

namespace ProjetoTaskManager.Infrastructure.Repositories
{
    public class CardRepository : BaseRepository<Card>, ICardRepository
    {
        public CardRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Card>> GetByUserIdAsync(int userId) =>
            await _context.Cards
                .Where(c => c.UserId == userId)
                .ToListAsync();

        public async Task<Card?> GetByIdAndUserIdAsync(int id, int userId) =>
            await _context.Cards
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
    }
}