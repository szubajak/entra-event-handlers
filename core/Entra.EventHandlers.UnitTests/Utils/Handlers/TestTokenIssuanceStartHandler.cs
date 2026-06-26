using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Handlers.Base;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.UnitTests.Utils.Handlers;

public class TestTokenIssuanceStartHandler(ILogger logger)
    : TokenIssuanceStartHandlerBase(logger)
{
    public HandlerCoreTest CoreTest { get; } = new HandlerCoreTest();

    public TokenIssuanceStartResponse ResponseToReturn { get; set; } = new TokenIssuanceStartResponse
    { 
        Data = new TokenIssuanceStartResponsePayload()
    };

    protected override Task<TokenIssuanceStartResponse> HandleCoreAsync(
        TokenIssuanceStartEvent request,
        CancellationToken cancellationToken)
    {
        CoreTest.Record(cancellationToken);
        return Task.FromResult(ResponseToReturn);
    }
}
