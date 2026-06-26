using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Handlers.Base;
using Entra.EventHandlers.Interfaces;
using Entra.EventHandlers.Protocol.PasswordSubmit;
using Entra.EventHandlers.TestHelpers;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.UnitTests.Utils.Handlers;

public class TestPasswordSubmitHandler(ILogger logger, IPasswordContextCryptoService cryptoService)
    : PasswordSubmitHandlerBase(logger, cryptoService)
{
    public HandlerCoreTest CoreTest { get; } = new HandlerCoreTest();

    public DecryptedPasswordContext? PassedDecryptedPasswordContext { get; set; }

    public PasswordSubmitResponse ResponseToReturn { get; set; } = new PasswordSubmitResponse
    { 
        Data = new PasswordSubmitResponsePayload
        { 
            Nonce = "some-nonce"
        }
    };

    protected override Task<PasswordSubmitResponse> HandleCoreAsync(
        PasswordSubmitEvent request,
        DecryptedPasswordContext decrypted,
        CancellationToken cancellationToken)
    {
        CoreTest.Record(cancellationToken);
        PassedDecryptedPasswordContext = decrypted;
        return Task.FromResult(ResponseToReturn);
    }
}
