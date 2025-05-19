using BetManSystem.Common.Entities;
using BetManSystem.Common.Enums;
using BetManSystem.DataAccess.Context;
using BetManSystem.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace BetManSystem.DataAccess.Repositories
{
    public class PlayerExternalAccountRepository : IPlayerExternalAccountRepository
    {
        private readonly BetManDbContext _db;
        public PlayerExternalAccountRepository(BetManDbContext db) => _db = db;

        public async Task<PlayerExternalAccount> GetByProviderAndExternalIdAsync(ProviderType provider, string externalId) => await
            _db.PlayerExternalAccounts
               .FirstOrDefaultAsync(a => a.Provider == provider && a.ExternalPlayerId == externalId);

        public async Task AddAsync(PlayerExternalAccount entity)
        {
            _db.PlayerExternalAccounts.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(PlayerExternalAccount entity)
        {
            _db.PlayerExternalAccounts.Update(entity);
            await _db.SaveChangesAsync();
        }
    }
}
