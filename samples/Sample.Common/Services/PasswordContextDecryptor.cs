using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Protocol.PasswordSubmit;

namespace Sample.Common.Services;

public class PasswordContextDecryptor : IPasswordContextDecryptor
{
    public DecryptedPasswordContext Decrypt(string encryptedPasswordContext) =>
        new()
        {
            Username = "jaub.szubarga@gmail.com",
            Password = "0000",
            Nonce = "some-nonce"
        };
}
