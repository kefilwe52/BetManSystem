using BetManSystem.Common.Entities;
using BetManSystem.Common.Enums;

namespace BetManSystem.Application.Interfaces.Services
{
    public interface IPlayerExternalAccountService
    {
        Task<PlayerExternalAccount> GetAsync(ProviderType provider, string externalPlayerId);
        Task<PlayerExternalAccount> CreateAsync(PlayerExternalAccount account);
        Task<PlayerExternalAccount> UpdateAsync(PlayerExternalAccount account);
    }
}
