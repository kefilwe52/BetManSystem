using BetManSystem.Application.Interfaces.Adapters;
using BetManSystem.Application.Interfaces.Services;
using BetManSystem.Common.Enums;
using BetManSystem.Common.Models;
using Microsoft.Extensions.Logging;

namespace BetManSystem.Integrations.Adapters
{
    public class BetWayWalletAdapter
        : BaseWalletAdapter<BetWayWalletAdapter>, IWalletIntegrationAdapter
    {
        protected override ProviderType Provider => ProviderType.BetWay;

        public BetWayWalletAdapter(
            HttpClient http,
            IMessageLogService log,
            ILogger<BetWayWalletAdapter> logger)
            : base(http, log, logger)
        { }

        public Task<WalletTransactionResponse> AuthenticatePlayerAsync(WalletTransactionRequest req)
            => ExecuteEwalletAsync(TransactionType.Authenticate, "/v1/auth", req);

        public Task<WalletTransactionResponse> GetPlayerBalanceAsync(WalletTransactionRequest req)
            => ExecuteEwalletAsync(TransactionType.GetBalance, "/v1/balance", req);

        public Task<WalletTransactionResponse> DebitPlayerAsync(WalletTransactionRequest req)
            => ExecuteEwalletAsync(TransactionType.Debit, "/v1/debit", req);

        public Task<WalletTransactionResponse> CreditPlayerAsync(WalletTransactionRequest req)
            => ExecuteEwalletAsync(TransactionType.Credit, "/v1/credit", req);

        public Task<WalletTransactionResponse> RefundPlayerAsync(WalletTransactionRequest req)
            => ExecuteEwalletAsync(TransactionType.Refund, "/v1/refund", req);
    }
}
