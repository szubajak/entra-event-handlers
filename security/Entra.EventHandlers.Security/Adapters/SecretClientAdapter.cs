using Azure.Security.KeyVault.Secrets;
using Entra.EventHandlers.Security.Clients;

namespace Entra.EventHandlers.Security.Adapters;

public class SecretClientAdapter(SecretClient client) : ISecretClient
{
    private readonly SecretClient _client = client;

    public async Task<KeyVaultSecret> GetSecretAsync(string name, CancellationToken cancellationToken = default)
    {
        var response = await _client.GetSecretAsync(name, cancellationToken: cancellationToken);
        return response.Value;
    }
}
