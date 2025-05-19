namespace BetManSystem.Common.Models
{
    public class AuthenticationResponse
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
