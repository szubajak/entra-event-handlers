using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Builders;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Utils;

public class TestAttributeCollectionStartHandler : TestHandlerBase, IAttributeCollectionStartHandler
{
    public Task<AttributeCollectionStartResponse> Handle(
        AttributeCollectionStartEvent request,
        CancellationToken cancellationToken = default)
    {
        CapturedCancellationToken = cancellationToken;

        return Task.FromResult(
            EntraEventResponses
                .AttributeCollectionStart()
                .ContinueWithDefaultBehavior()
                .Build());
    }
}
