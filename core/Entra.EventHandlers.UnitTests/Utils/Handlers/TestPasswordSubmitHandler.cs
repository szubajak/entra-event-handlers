using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Protocol.PasswordSubmit;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Handlers.Base;
using Entra.EventHandlers.TestData;
using Entra.EventHandlers.TestHelpers;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.UnitTests.Utils.Handlers;

public class TestPasswordSubmitHandler(ILogger logger, IPasswordContextDecryptor decryptor)
    : PasswordSubmitHandlerBase(logger, decryptor)
{
    public HandlerCoreTest CoreTest { get; } = new HandlerCoreTest();

    public DecryptedPasswordContext? PassedDecryptedPasswordContext { get; set; }

    public PasswordSubmitResponse ResponseToReturn { get; set; } = TestResponses.CreatePasswordSubmitResponse();

    protected override Task<PasswordSubmitResponse> HandleCoreAsync(
        PasswordSubmitEvent request,
        DecryptedPasswordContext decrypted,
        CancellationToken cancellationToken = default)
    {
        CoreTest.Record(cancellationToken);
        PassedDecryptedPasswordContext = decrypted;
        return Task.FromResult(ResponseToReturn);
    }
}
