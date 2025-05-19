using BetManSystem.Common.Entities;

namespace BetManSystem.DataAccess.IRepositories
{
    public interface IPlayerRepository
    {
        Task<Player> GetByIdAsync(Guid id);
        Task<Player> GetByEmailAsync(string email);
        Task AddAsync(Player entity);
        Task UpdateAsync(Player entity);
        Task DeleteAsync(Guid id);
    }
}
