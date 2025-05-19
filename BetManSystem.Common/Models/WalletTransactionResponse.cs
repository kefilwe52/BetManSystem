namespace BetManSystem.Common.Models
{
    public class WalletTransactionResponse
    {
        public bool Success { get; set; }
        public string ResponseCode { get; set; }
        public string ErrorMessage { get; set; }
        public decimal? Balance { get; set; }
    }
}
