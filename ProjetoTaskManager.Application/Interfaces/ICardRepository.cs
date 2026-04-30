using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjetoTaskManager.Domain.Entities;

namespace ProjetoTaskManager.Application.Interfaces
{
    public interface ICardRepository : IRepository<Card>
    {
        Task<IEnumerable<Card>> GetByUserIdAsync (int UserId);
        Task<Card?> GetByIdAndUserIdAsync(int id, int userId);
    }
}