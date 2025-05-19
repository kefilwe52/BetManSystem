using BetManSystem.Application.Interfaces.Adapters;
using BetManSystem.Application.Interfaces.Services;
using BetManSystem.Common.Enums;
using BetManSystem.Common.Models;
using Microsoft.Extensions.Logging;

namespace BetManSystem.Application.Services
{
    public class WalletTransactionService : IWalletTransactionService
    {
        private readonly IWalletIntegrationAdapterFactory _factory;
        private readonly ILogger<WalletTransactionService> _logger;

        public WalletTransactionService(
            IWalletIntegrationAdapterFactory factory,
            ILogger<WalletTransactionService> logger)
        {
            _factory = factory;
            _logger = logger;
        }

        public async Task<WalletTransactionResponse> GetBalanceAsync(
            ProviderType provider, WalletTransactionRequest request)
        {
            try
            {
                var adapter = _factory.GetAdapter(provider);
                return await adapter.GetPlayerBalanceAsync(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in GetBalanceAsync for Provider={Provider}, PlayerId={PlayerId}",
                    provider, request.PlayerId);
                throw;
            }
        }

        public async Task<WalletTransactionResponse> DebitAsync(
            ProviderType provider, WalletTransactionRequest request)
        {
            try
            {
                var adapter = _factory.GetAdapter(provider);
                return await adapter.DebitPlayerAsync(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in DebitAsync for Provider={Provider}, PlayerId={PlayerId}, Amount={Amount}",
                    provider, request.PlayerId, request.Amount);
                throw;
            }
        }

        public async Task<WalletTransactionResponse> CreditAsync(
            ProviderType provider, WalletTransactionRequest request)
        {
            try
            {
                var adapter = _factory.GetAdapter(provider);
                return await adapter.CreditPlayerAsync(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in CreditAsync for Provider={Provider}, PlayerId={PlayerId}, Amount={Amount}",
                    provider, request.PlayerId, request.Amount);
                throw;
            }
        }

        public async Task<WalletTransactionResponse> RefundAsync(
            ProviderType provider, WalletTransactionRequest request)
        {
            try
            {
                var adapter = _factory.GetAdapter(provider);
                return await adapter.RefundPlayerAsync(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in RefundAsync for Provider={Provider}, PlayerId={PlayerId}, Amount={Amount}",
                    provider, request.PlayerId, request.Amount);
                throw;
            }
        }
    }
}
