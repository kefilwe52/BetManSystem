using BetManSystem.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetManSystem.Common.Entities
{
    public class PlayerExternalAccount
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(PlayerId))]
        public Guid PlayerId { get; set; }
        public Player Player { get; set; }
        public ProviderType Provider { get; set; }
        public string ExternalPlayerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }
}
