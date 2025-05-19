using BetManSystem.Application.Interfaces.Services;
using BetManSystem.Common.Entities;
using BetManSystem.DataAccess.IRepositories;
using Microsoft.Extensions.Logging;

namespace BetManSystem.Application.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _repo;
        private readonly ILogger<PlayerService> _logger;

        public PlayerService(
            IPlayerRepository repo,
            ILogger<PlayerService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<Player> GetByIdAsync(Guid id)
        {
            try
            {
                return await _repo.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Player by Id {PlayerId}", id);
                throw;
            }
        }

        public async Task<Player> GetByEmailAsync(string email)
        {
            try
            {
                return await _repo.GetByEmailAsync(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Player by Email {Email}", email);
                throw;
            }
        }

        public async Task<Player> CreateAsync(Player player)
        {
            try
            {
                await _repo.AddAsync(player);
                _logger.LogInformation("create new Player {PlayerId}", player.Id);
                return player;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Player {Player}", player);
                throw;
            }
        }

        public async Task<Player> UpdateAsync(Player player)
        {
            try
            {
                await _repo.UpdateAsync(player);
                _logger.LogInformation("Updated Player {PlayerId}", player.Id);
                return player;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Player {PlayerId}", player.Id);
                throw;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                await _repo.DeleteAsync(id);
                _logger.LogInformation("Deleted Player {PlayerId}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Player {PlayerId}", id);
                throw;
            }
        }
    }
}
