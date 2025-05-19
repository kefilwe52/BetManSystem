using BetManSystem.Application.Interfaces.Adapters;
using BetManSystem.Common.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace BetManSystem.Integrations.Adapters
{
    public class WalletIntegrationAdapterFactory : IWalletIntegrationAdapterFactory
    {
        private readonly IServiceProvider _services;

        public WalletIntegrationAdapterFactory(IServiceProvider services)
        {
            _services = services;
        }

        public IWalletIntegrationAdapter GetAdapter(ProviderType provider)
        {
            return provider switch
            {
                ProviderType.BetWay => _services.GetRequiredService<BetWayWalletAdapter>(),
                ProviderType.BetGames => _services.GetRequiredService<BetGamesWalletAdapter>(),
                _ => throw new NotSupportedException($"Provider '{provider}' is not supported.")
            };
        }
    }
}
