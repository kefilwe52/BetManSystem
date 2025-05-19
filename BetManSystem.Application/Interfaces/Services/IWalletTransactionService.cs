using BetManSystem.Common.Enums;
using BetManSystem.Common.Models;

namespace BetManSystem.Application.Interfaces.Services
{
    public interface IWalletTransactionService
    {
        Task<WalletTransactionResponse> GetBalanceAsync(ProviderType provider, WalletTransactionRequest request);
        Task<WalletTransactionResponse> DebitAsync(ProviderType provider, WalletTransactionRequest request);
        Task<WalletTransactionResponse> CreditAsync(ProviderType provider, WalletTransactionRequest request);
        Task<WalletTransactionResponse> RefundAsync(ProviderType provider, WalletTransactionRequest request);
    }
}
