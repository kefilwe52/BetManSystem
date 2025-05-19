using BetManSystem.Application.Interfaces.Adapters;
using BetManSystem.Application.Interfaces.Services;
using BetManSystem.Common.Entities;
using BetManSystem.Common.Enums;
using BetManSystem.Common.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;

namespace BetManSystem.Integrations.Adapters
{
    public abstract class BaseWalletAdapter<TAdapter>
         where TAdapter : class, IWalletIntegrationAdapter
    {
        private readonly HttpClient _http;
        private readonly IMessageLogService _log;
        private readonly ILogger<TAdapter> _logger;

        protected abstract ProviderType Provider { get; }

        protected BaseWalletAdapter(
            HttpClient http,
            IMessageLogService log,
            ILogger<TAdapter> logger)
        {
            _http = http;
            _log = log;
            _logger = logger;
        }

        protected async Task<WalletTransactionResponse> ExecuteEwalletAsync(
            TransactionType txType,
            string relativeUrl,
            WalletTransactionRequest request)
        {
            var entry = new MessageTransmissionLog
            {
                Id = Guid.NewGuid(),
                Provider = Provider.ToString(),
                PlayerId = request.PlayerId,
                TransactionType = txType,
                RequestPayload = JsonSerializer.Serialize(request),
                Timestamp = DateTime.Now
            };

            WalletTransactionResponse result = null;
            string responseJson = null;
            string status = "Success";
            string errorMsg = null;

            try
            {
                using var httpResp = await _http.PostAsJsonAsync(relativeUrl, request);
                responseJson = await httpResp.Content.ReadAsStringAsync();

                if (httpResp.IsSuccessStatusCode)
                {
                    result = JsonSerializer.Deserialize<WalletTransactionResponse>(
                        responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );
                }
                else
                {
                    status = "Failure";
                    errorMsg = responseJson;
                    result = new WalletTransactionResponse
                    {
                        Success = false,
                        ErrorMessage = errorMsg
                    };
                }

                return result;
            }
            catch (Exception ex)
            {
                status = "Failure";
                errorMsg = ex.Message;
                _logger.LogError(
                    ex,
                    "{Provider} {Tx} failed for Player {PlayerId}",
                    Provider, txType, request.PlayerId
                );
                throw;
            }
            finally
            {
                entry.ResponsePayload = responseJson;
                entry.Status = status;
                entry.ErrorMessage = errorMsg;
                await _log.LogAsync(entry);
            }
        }
    }
}
