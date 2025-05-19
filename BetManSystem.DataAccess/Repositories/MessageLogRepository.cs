using BetManSystem.Common.Entities;
using BetManSystem.DataAccess.Context;
using BetManSystem.DataAccess.IRepositories;

namespace BetManSystem.DataAccess.Repositories
{
    public class MessageLogRepository : IMessageLogRepository
    {
        private readonly BetManDbContext _db;
        public MessageLogRepository(BetManDbContext db) => _db = db;

        public async Task LogAsync(MessageTransmissionLog entry)
        {
            _db.MessageTransmissionLogs.Add(entry);
            await _db.SaveChangesAsync();
        }
    }
}
