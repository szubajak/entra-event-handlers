using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Protocol.PasswordSubmit;

namespace Sample.Common.Services;

public class PasswordContextDecryptor : IPasswordContextDecryptor
{
    public Task<DecryptedPasswordContext> DecryptAsync(string encryptedPasswordContext, CancellationToken cancellationToken = default) =>
        Task.FromResult(
            new DecryptedPasswordContext
            {
                Username = "jaub.szubarga@gmail.com",
                Password = "0000",
                Nonce = "some-nonce"
            });
}
