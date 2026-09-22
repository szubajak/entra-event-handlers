using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Entra.EventHandlers.Security.Clients;
using Entra.EventHandlers.Security.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Entra.EventHandlers.Security.Providers;

public class KeyVaultCertificateProvider(
    ILogger<KeyVaultCertificateProvider> logger,
    IOptions<KeyVaultCertificateOptions> options,
    ISecretClient secretClient)
    : ICertificateProvider
{
    private readonly ILogger<KeyVaultCertificateProvider> _logger = logger;
    private readonly KeyVaultCertificateOptions _options = options.Value;
    private readonly ISecretClient _secretClient = secretClient;

    private RSA _cachedRsa = null!;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public async Task<RSA> GetRsaAsync(CancellationToken cancellationToken = default)
    {
        if (_cachedRsa != null)
        {
            return _cachedRsa;
        }

        await _semaphore.WaitAsync(cancellationToken);

        try
        {
            if (_cachedRsa != null)
            {
                return _cachedRsa;
            }

            _logger.LogDebug(
                "Retrieving certificate secret '{CertificateName}' from Key Vault.",
                _options.CertificateName);

            var client = new SecretClient(new Uri(_options.VaultUrl), new DefaultAzureCredential());

            KeyVaultSecret secret;
            try
            {
                secret = await _secretClient.GetSecretAsync(_options.CertificateName, cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to retrieve certificate secret '{CertificateName}' from Key Vault.",
                    _options.CertificateName);

                throw;
            }

            _logger.LogDebug(
                "Certificate secret '{CertificateName}' loaded. Extracting RSA private key from PKCS#12 blob.",
                _options.CertificateName);

            if (string.IsNullOrEmpty(secret.Value))
            {
                throw new InvalidOperationException($"Secret '{_options.CertificateName}' is empty.");
            }

            var pfxBytes = Convert.FromBase64String(secret.Value);

            var certificate = X509CertificateLoader.LoadPkcs12(pfxBytes, null, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet);

            if (!certificate.HasPrivateKey)
            {
                throw new InvalidOperationException($"Certificate '{_options.CertificateName}' does not contain a private key.");
            }

            _cachedRsa = certificate.GetRSAPrivateKey()
                ?? throw new InvalidOperationException($"Failed to extract RSA private key from certificate '{_options.CertificateName}'.");

            _logger.LogDebug(
                "RSA private key extracted successfully from certificate '{CertificateName}'.",
                _options.CertificateName);

            return _cachedRsa;
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
