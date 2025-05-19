using BetManSystem.Application.Interfaces.Adapters;
using BetManSystem.Application.Interfaces.Services;
using BetManSystem.Common.Enums;
using BetManSystem.Common.Models;
using Microsoft.Extensions.Logging;

namespace BetManSystem.Integrations.Adapters
{
    public class BetGamesWalletAdapter
        : BaseWalletAdapter<BetGamesWalletAdapter>, IWalletIntegrationAdapter
    {
        protected override ProviderType Provider => ProviderType.BetGames;

        public BetGamesWalletAdapter(
            HttpClient http,
            IMessageLogService log,
            ILogger<BetGamesWalletAdapter> logger)
            : base(http, log, logger)
        { }

        public Task<WalletTransactionResponse> AuthenticatePlayerAsync(WalletTransactionRequest req)
            => ExecuteEwalletAsync(TransactionType.Authenticate, "/auth/player", req);

        public Task<WalletTransactionResponse> GetPlayerBalanceAsync(WalletTransactionRequest req)
            => ExecuteEwalletAsync(TransactionType.GetBalance, "/player/balance", req);

        public Task<WalletTransactionResponse> DebitPlayerAsync(WalletTransactionRequest req)
            => ExecuteEwalletAsync(TransactionType.Debit, "/wallet/debit", req);

        public Task<WalletTransactionResponse> CreditPlayerAsync(WalletTransactionRequest req)
            => ExecuteEwalletAsync(TransactionType.Credit, "/wallet/credit", req);

        public Task<WalletTransactionResponse> RefundPlayerAsync(WalletTransactionRequest req)
            => ExecuteEwalletAsync(TransactionType.Refund, "/wallet/refund", req);
    }
}
