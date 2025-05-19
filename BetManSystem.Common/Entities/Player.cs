using System.ComponentModel.DataAnnotations;

namespace BetManSystem.Common.Entities
{
    public class Player
    {
        [Key]
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<PlayerExternalAccount> ExternalAccounts { get; set; }
    }
}
