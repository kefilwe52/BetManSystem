using BetManSystem.Application.Interfaces.Services;
using BetManSystem.Common.Entities;
using BetManSystem.DataAccess.IRepositories;
using Microsoft.Extensions.Logging;

namespace BetManSystem.Application.Services
{
    public class MessageLogService : IMessageLogService
    {
        private readonly IMessageLogRepository _repo;
        private readonly ILogger<MessageLogService> _logger;

        public MessageLogService(
            IMessageLogRepository repo,
            ILogger<MessageLogService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task LogAsync(MessageTransmissionLog entry)
        {
            try
            {
                if (entry.Timestamp == default)
                    entry.Timestamp = DateTime.Now;

                await _repo.LogAsync(entry);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add MessageTransmissionLog: {@Entry}", entry);
            }
        }
    }
}
