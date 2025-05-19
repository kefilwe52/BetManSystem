using BetManSystem.Common.Entities;
using BetManSystem.Common.Enums;

namespace BetManSystem.DataAccess.IRepositories
{
    public interface IPlayerExternalAccountRepository
    {
        Task<PlayerExternalAccount> GetByProviderAndExternalIdAsync(ProviderType provider, string externalId);
        Task AddAsync(PlayerExternalAccount entity);
        Task UpdateAsync(PlayerExternalAccount entity);
    }
}
