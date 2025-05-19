using BetManSystem.Application.Interfaces.Services;
using BetManSystem.Common.Entities;
using BetManSystem.Common.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BetManSystem.Api.Controllers
{
    [ApiController]
    [Route("api/external-accounts")]
    public class PlayerExternalAccountController : ControllerBase
    {
        private readonly IPlayerExternalAccountService _externalAccount;
        private readonly ILogger<PlayerExternalAccountController> _logger;

        public PlayerExternalAccountController(
            IPlayerExternalAccountService externalAccount,
            ILogger<PlayerExternalAccountController> logger)
        {
            _externalAccount = externalAccount;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PlayerExternalAccount), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(
            [FromQuery] ProviderType provider,
            [FromQuery] string externalPlayerId)
        {
            try
            {
                var account = await _externalAccount.GetAsync(provider, externalPlayerId);
                if (account == null)
                    return NotFound();
                return Ok(account);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error in Get ExternalAccount for {Provider}:{PlayerId}",
                    provider, externalPlayerId);
                return StatusCode(500, "An error occurred while fetching the external account.");
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(PlayerExternalAccount), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] PlayerExternalAccount account)
        {
            try
            {
                var created = await _externalAccount.CreateAsync(account);
                return CreatedAtAction(
                    nameof(Get),
                    new { provider = created.Provider, externalPlayerId = created.ExternalPlayerId },
                    created);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error creating ExternalAccount {@Account}",
                    account);
                return BadRequest("Failed to create external account.");
            }
        }
    }
}
