using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Builders;
using Entra.EventHandlers.TestHelpers;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Utils;

public class TestTokenIssuanceStartHandler : TestHandlerBase, ITokenIssuanceStartHandler
{
    public Task<TokenIssuanceStartResponse> HandleAsync(
        TokenIssuanceStartEvent request,
        CancellationToken cancellationToken = default)
    {
        request.Validate();

        WasCalled = true;
        CapturedCancellationToken = cancellationToken;

        return Task.FromResult(
            EntraEventResponses
                .TokenIssuanceStart()
                .ProvideClaimsForToken([])
                .Build());
    }
}
