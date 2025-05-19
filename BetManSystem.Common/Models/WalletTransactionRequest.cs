namespace BetManSystem.Common.Models
{
    public class WalletTransactionRequest
    {
        public string PlayerId { get; set; }
        public decimal Amount { get; set; }
        public string TransactionReference { get; set; }
        public string Currency { get; set; }
        public DateTime? TransactionTime { get; set; }
    }
}
