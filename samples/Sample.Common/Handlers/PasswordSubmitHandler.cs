using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Builders;
using Entra.EventHandlers.Handlers.Base;
using Entra.EventHandlers.Interfaces;
using Entra.EventHandlers.Protocol.PasswordSubmit;
using Microsoft.Extensions.Logging;

namespace Sample.Common.Handlers;

public class PasswordSubmitHandler(ILogger<PasswordSubmitHandler> logger, IPasswordContextCryptoService cryptoService)
    : PasswordSubmitHandlerBase(logger, cryptoService)
{
    protected override Task<PasswordSubmitResponse> HandleCoreAsync(
        PasswordSubmitEvent request,
        DecryptedPasswordContext decrypted,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(
            EntraEventResponses
                .PasswordSubmit()
                .WithNonce(decrypted.Nonce)
                .MigratePassword()
                .Build());
    }
}