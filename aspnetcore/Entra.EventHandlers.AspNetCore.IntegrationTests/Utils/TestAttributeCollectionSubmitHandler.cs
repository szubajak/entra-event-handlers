using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Builders;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Utils;

public class TestAttributeCollectionSubmitHandler : TestHandlerBase, IAttributeCollectionSubmitHandler
{
    public Task<AttributeCollectionSubmitResponse> Handle(
        AttributeCollectionSubmitEvent request,
        CancellationToken cancellationToken = default)
    {
        CapturedCancellationToken = cancellationToken;

        return Task.FromResult(
            EntraEventResponses
                .AttributeCollectionSubmit()
                .ContinueWithDefaultBehavior()
                .Build());
    }
}
