using BetManSystem.Common.Entities;

namespace BetManSystem.Application.Interfaces.Services
{
    public interface IPlayerService
    {
        Task<Player> GetByIdAsync(Guid id);
        Task<Player> GetByEmailAsync(string email);
        Task<Player> CreateAsync(Player player);
        Task<Player> UpdateAsync(Player player);
        Task DeleteAsync(Guid id);
    }
}
