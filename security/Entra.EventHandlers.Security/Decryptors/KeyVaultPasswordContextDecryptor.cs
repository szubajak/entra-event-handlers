using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Protocol.PasswordSubmit;
using Entra.EventHandlers.Security.Providers;
using Jose;
using System.Text.Json;

namespace Entra.EventHandlers.Security.Decryptors;

public sealed class KeyVaultPasswordContextDecryptor(IKeyVaultCertificateProvider certificateProvider)
    : IPasswordContextDecryptor
{
    private readonly IKeyVaultCertificateProvider _certificateProvider = certificateProvider;

    public async Task<DecryptedPasswordContext> DecryptAsync(string encryptedPasswordContext, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(encryptedPasswordContext);

        try
        {
            var rsa = await _certificateProvider.GetRsaAsync(cancellationToken)
                ?? throw new EntraSecurityException("The RSA key required to decrypt the password context is unavailable.");

            var decrypted = JWT.Decode(encryptedPasswordContext, rsa);

            var json = JWT.Decode(decrypted, null, JwsAlgorithm.none);

            var payload = JsonSerializer.Deserialize<PasswordContextPayload>(json)
                ?? throw new EntraDeserializationException("The decrypted password context payload is empty.");

            payload.Validate();

            return new DecryptedPasswordContext
            {
                Nonce = payload.Nonce,
                Username = payload.Username,
                Password = payload.Password
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (EntraException)
        {
            throw;
        }
        catch (JsonException ex)
        {
            throw new EntraDeserializationException("Unable to deserialize the decrypted password context.", ex);
        }
        catch (Exception ex)
        {
            throw new EntraSecurityException("Unable to decrypt the password context.", ex);
        }
    }
}
