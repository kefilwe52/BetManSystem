using BetManSystem.Common.Entities;
using BetManSystem.DataAccess.Context;
using BetManSystem.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace BetManSystem.DataAccess.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly BetManDbContext _db;
        public PlayerRepository(BetManDbContext db)
        {
            _db = db;
        }

        public async Task<Player> GetByIdAsync(Guid id)
        {
            return await _db.Players
                            .Include(p => p.ExternalAccounts)
                            .SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Player> GetByEmailAsync(string email)
        {
            return await _db.Players
                            .Include(p => p.ExternalAccounts)
                            .SingleOrDefaultAsync(p =>
                                p.Email.Equals(email.Trim(), StringComparison.CurrentCultureIgnoreCase));
        }

        public async Task AddAsync(Player entity)
        {
            _db.Players.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Player entity)
        {
            _db.Players.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _db.Players.FindAsync(id);
            if (entity != null)
            {
                _db.Players.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }
    }
}
