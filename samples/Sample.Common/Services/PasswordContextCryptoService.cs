using Entra.EventHandlers.Interfaces;
using Entra.EventHandlers.Protocol.PasswordSubmit;

namespace Sample.Common.Services;

public class PasswordContextCryptoService : IPasswordContextCryptoService
{
    public DecryptedPasswordContext Decrypt(string encryptedPasswordContext) =>
        new()
        {
            Username = "jaub.szubarga@gmail.com",
            Password = "0000",
            Nonce = "some-nonce"
        };
}
