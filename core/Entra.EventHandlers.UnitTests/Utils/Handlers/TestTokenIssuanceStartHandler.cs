using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Handlers.Base;
using Entra.EventHandlers.TestData;
using Entra.EventHandlers.TestHelpers;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.UnitTests.Utils.Handlers;

public class TestTokenIssuanceStartHandler(ILogger logger)
    : TokenIssuanceStartHandlerBase(logger)
{
    public HandlerCoreTest CoreTest { get; } = new HandlerCoreTest();

    public TokenIssuanceStartResponse ResponseToReturn { get; set; } = TestResponses.CreateTokenIssuanceStartResponse();

    protected override Task<TokenIssuanceStartResponse> HandleCoreAsync(
        TokenIssuanceStartEvent request,
        CancellationToken cancellationToken = default)
    {
        CoreTest.Record(cancellationToken);
        return Task.FromResult(ResponseToReturn);
    }
}
