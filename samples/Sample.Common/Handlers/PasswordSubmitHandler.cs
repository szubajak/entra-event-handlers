using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Protocol.PasswordSubmit;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Builders;
using Entra.EventHandlers.Handlers.Base;
using Microsoft.Extensions.Logging;

namespace Sample.Common.Handlers;

public class PasswordSubmitHandler(ILogger<PasswordSubmitHandler> logger, IPasswordContextDecryptor decryptor)
    : PasswordSubmitHandlerBase(logger, decryptor)
{
    protected override Task<PasswordSubmitResponse> HandleCoreAsync(
        PasswordSubmitEvent request,
        DecryptedPasswordContext decrypted,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(
            EntraEventResponses
                .PasswordSubmit()
                .WithNonce(decrypted.Nonce)
                .MigratePassword()
                .Build());
    }
}