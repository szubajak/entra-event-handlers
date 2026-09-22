using Azure.Security.KeyVault.Secrets;

namespace Entra.EventHandlers.Security.Clients;

public interface ISecretClient
{
    Task<KeyVaultSecret> GetSecretAsync(string name, CancellationToken cancellationToken = default);
}