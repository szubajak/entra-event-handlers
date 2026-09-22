using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Entra.EventHandlers.Security.Adapters;
using Entra.EventHandlers.Security.Clients;
using Entra.EventHandlers.Security.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Entra.EventHandlers.Security.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEntraEventHandlersSecurity(this IServiceCollection services)
    {
        services.AddSingleton<ISecretClient>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<KeyVaultCertificateOptions>>().Value;

            var client = new SecretClient(
                new Uri(options.VaultUrl),
                new DefaultAzureCredential()
            );

            return new SecretClientAdapter(client);
        });

        return services;
    }
}