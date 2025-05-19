using BetManSystem.Common.Enums;
using BetManSystem.Common.Models;

namespace BetManSystem.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthenticationResponse> AuthenticateAsync(
          ProviderType provider,
          WalletTransactionRequest request);
    }
}
