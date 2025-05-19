using BetManSystem.Application.Interfaces.Services;
using BetManSystem.Common.Enums;
using BetManSystem.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BetManSystem.Api.Controllers
{
    [ApiController]
    [Route("api/wallet/{provider}")]
    [Authorize]
    public class WalletController : ControllerBase
    {
        private readonly IWalletTransactionService _walletService;
        private readonly ILogger<WalletController> _logger;

        public WalletController(
            IWalletTransactionService walletService,
            ILogger<WalletController> logger)
        {
            _walletService = walletService;
            _logger = logger;
        }

        [HttpPost("balance")]
        [ProducesResponseType(typeof(WalletTransactionResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetBalance(
            [FromRoute] ProviderType provider,
            [FromBody] WalletTransactionRequest request)
        {
            try
            {
                var resp = await _walletService.GetBalanceAsync(provider, request);
                if (!resp.Success) return BadRequest(resp.ErrorMessage);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetBalance error for {Provider}:{PlayerId}", provider, request.PlayerId);
                return StatusCode(500, "Failed to get balance");
            }
        }

        [HttpPost("debit")]
        [ProducesResponseType(typeof(WalletTransactionResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Debit(
            [FromRoute] ProviderType provider,
            [FromBody] WalletTransactionRequest request)
        {
            try
            {
                var resp = await _walletService.DebitAsync(provider, request);
                if (!resp.Success) return BadRequest(resp.ErrorMessage);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Debit error for {Provider}:{PlayerId} Amount={Amount}",
                    provider, request.PlayerId, request.Amount);
                return StatusCode(500, "Debit failed");
            }
        }

        [HttpPost("credit")]
        [ProducesResponseType(typeof(WalletTransactionResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Credit(
            [FromRoute] ProviderType provider,
            [FromBody] WalletTransactionRequest request)
        {
            try
            {
                var resp = await _walletService.CreditAsync(provider, request);
                if (!resp.Success) return BadRequest(resp.ErrorMessage);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Credit error for {Provider}:{PlayerId} Amount={Amount}",
                    provider, request.PlayerId, request.Amount);
                return StatusCode(500, "Credit failed");
            }
        }

        [HttpPost("refund")]
        [ProducesResponseType(typeof(WalletTransactionResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Refund(
            [FromRoute] ProviderType provider,
            [FromBody] WalletTransactionRequest request)
        {
            try
            {
                var resp = await _walletService.RefundAsync(provider, request);
                if (!resp.Success) return BadRequest(resp.ErrorMessage);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Refund error for {Provider}:{PlayerId} Amount={Amount}",
                    provider, request.PlayerId, request.Amount);
                return StatusCode(500, "Refund failed");
            }
        }
    }
}
