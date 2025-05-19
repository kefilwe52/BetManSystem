using BetManSystem.Common.Models;

namespace BetManSystem.Application.Interfaces.Adapters
{
    public interface IWalletIntegrationAdapter
    {
        Task<WalletTransactionResponse> AuthenticatePlayerAsync(WalletTransactionRequest request);
        Task<WalletTransactionResponse> GetPlayerBalanceAsync(WalletTransactionRequest request);
        Task<WalletTransactionResponse> DebitPlayerAsync(WalletTransactionRequest request);
        Task<WalletTransactionResponse> CreditPlayerAsync(WalletTransactionRequest request);
        Task<WalletTransactionResponse> RefundPlayerAsync(WalletTransactionRequest request);
    }
}
