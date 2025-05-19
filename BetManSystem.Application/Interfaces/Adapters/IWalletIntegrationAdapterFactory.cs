using BetManSystem.Common.Enums;

namespace BetManSystem.Application.Interfaces.Adapters
{
    public interface IWalletIntegrationAdapterFactory
    {
        IWalletIntegrationAdapter GetAdapter(ProviderType provider);
    }
}
