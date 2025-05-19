using BetManSystem.Common.Entities;

namespace BetManSystem.DataAccess.IRepositories
{
    public interface IMessageLogRepository
    {
        Task LogAsync(MessageTransmissionLog entry);
    }
}
