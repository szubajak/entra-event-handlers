using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Builders;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Utils;

public class TestTokenIssuanceStartHandler : TestHandlerBase, ITokenIssuanceStartHandler
{
    public Task<TokenIssuanceStartResponse> Handle(
        TokenIssuanceStartEvent request,
        CancellationToken cancellationToken = default)
    {
        CapturedCancellationToken = cancellationToken;

        return Task.FromResult(
            EntraEventResponses
                .TokenIssuanceStart()
                .ProvideClaimsForToken([])
                .Build());
    }
}
