using BetManSystem.Common.Entities;

namespace BetManSystem.Application.Interfaces.Services
{
    public interface IMessageLogService
    {
        Task LogAsync(MessageTransmissionLog entry);
    }
}
