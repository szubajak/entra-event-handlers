using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Protocol.PasswordSubmit;
using Entra.EventHandlers.Security.Providers;

namespace Entra.EventHandlers.Security.Decryptors;

public sealed class KeyVaultPasswordContextDecryptor(IKeyVaultCertificateProvider certificateProvider)
    : IPasswordContextDecryptor
{
    private readonly IKeyVaultCertificateProvider _certificateProvider = certificateProvider;

    public DecryptedPasswordContext Decrypt(string encryptedPasswordContext)
    {
        throw new NotImplementedException();
    }
}
