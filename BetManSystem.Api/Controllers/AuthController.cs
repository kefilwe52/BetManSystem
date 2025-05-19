using BetManSystem.Application.Interfaces.Services;
using BetManSystem.Common.Enums;
using BetManSystem.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BetManSystem.Api.Controllers
{
    [ApiController]
    [Route("api/auth/{provider}")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IAuthService authService,
            ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthenticationResponse), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Login(
            [FromRoute] ProviderType provider,
            [FromBody] WalletTransactionRequest request)
        {
            try
            {
                var authResp = await _authService.AuthenticateAsync(provider, request);
                if (string.IsNullOrEmpty(authResp.Token))
                    return Unauthorized();
                return Ok(authResp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error authenticating {Provider}:{PlayerId}", provider, request.PlayerId);
                return StatusCode(500, "Authentication failed");
            }
        }
    }
}
