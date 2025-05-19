using BetManSystem.Application.Interfaces.Services;
using BetManSystem.Common.Entities;
using BetManSystem.Common.Enums;
using BetManSystem.DataAccess.IRepositories;
using Microsoft.Extensions.Logging;

namespace BetManSystem.Application.Services
{
    public class PlayerExternalAccountService : IPlayerExternalAccountService
    {
        private readonly IPlayerExternalAccountRepository _repo;
        private readonly ILogger<PlayerExternalAccountService> _logger;

        public PlayerExternalAccountService(
            IPlayerExternalAccountRepository repo,
            ILogger<PlayerExternalAccountService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<PlayerExternalAccount> GetAsync(
            ProviderType provider, string externalPlayerId)
        {
            try
            {
                return await _repo.GetByProviderAndExternalIdAsync(provider, externalPlayerId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error retrieving PlayerExternalAccount for {Provider}:{PlayerId}",
                    provider, externalPlayerId);
                throw;
            }
        }

        public async Task<PlayerExternalAccount> CreateAsync(
            PlayerExternalAccount account)
        {
            try
            {
                account.Id = Guid.NewGuid();
                account.CreatedAt = DateTime.UtcNow;
                account.LastLoginAt = DateTime.UtcNow;

                await _repo.AddAsync(account);

                _logger.LogInformation(
                    "Created PlayerExternalAccount {@Account}",
                    account);

                return account;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error creating PlayerExternalAccount {@Account}",
                    account);
                throw;
            }
        }

        public async Task<PlayerExternalAccount> UpdateAsync(
            PlayerExternalAccount account)
        {
            try
            {
                account.LastLoginAt = DateTime.UtcNow;

                await _repo.UpdateAsync(account);

                _logger.LogInformation(
                    "Updated PlayerExternalAccount {@Account}",
                    account);

                return account;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error updating PlayerExternalAccount {@Account}",
                    account);
                throw;
            }
        }
    }
}
