using BetManSystem.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace BetManSystem.Common.Entities
{
    public class MessageTransmissionLog
    {
        [Key]
        public Guid Id { get; set; }
        public string Provider { get; set; }
        public string PlayerId { get; set; }
        public TransactionType TransactionType { get; set; }
        public string RequestPayload { get; set; }
        public string ResponsePayload { get; set; }
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
