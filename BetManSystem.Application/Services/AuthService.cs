
using BetManSystem.Application.Interfaces.Adapters;
using BetManSystem.Application.Interfaces.Services;
using BetManSystem.Common.Entities;
using BetManSystem.Common.Enums;
using BetManSystem.Common.Models;
using BetManSystem.Common.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BetManSystem.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IWalletIntegrationAdapterFactory _adapterFactory;
        private readonly IPlayerExternalAccountService _externalAccountSvc;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IWalletIntegrationAdapterFactory adapterFactory,
            IPlayerExternalAccountService externalAccountSvc,
            JwtSettings jwtSettings,
            ILogger<AuthService> logger)
        {
            _adapterFactory = adapterFactory;
            _externalAccountSvc = externalAccountSvc;
            _jwtSettings = jwtSettings;
            _logger = logger;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(
            ProviderType provider,
            WalletTransactionRequest request)
        {
            try
            {
                var adapter = _adapterFactory.GetAdapter(provider);
                var resp = await adapter.AuthenticatePlayerAsync(request);

                if (!resp.Success)
                {
                    _logger.LogWarning(
                        "External auth failed for {Provider}:{PlayerId} – {Error}",
                        provider, request.PlayerId, resp.ErrorMessage);
                    return new AuthenticationResponse();
                }

                var existing = await _externalAccountSvc.GetAsync(provider, request.PlayerId);
                if (existing == null)
                {
                    await _externalAccountSvc.CreateAsync(new PlayerExternalAccount
                    {
                        Provider = provider,
                        ExternalPlayerId = request.PlayerId
                    });
                }
                else
                {
                    await _externalAccountSvc.UpdateAsync(existing);
                }

                var tokenValue = GenerateJwtToken(request.PlayerId);
                return new AuthenticationResponse
                {
                    Token = tokenValue,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpiryMinutes)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "AuthenticateAsync error for {Provider}:{PlayerId}",
                    provider, request.PlayerId);
                throw;
            }
        }

        private string GenerateJwtToken(string playerId)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier,     playerId),
                new(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpiryMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
