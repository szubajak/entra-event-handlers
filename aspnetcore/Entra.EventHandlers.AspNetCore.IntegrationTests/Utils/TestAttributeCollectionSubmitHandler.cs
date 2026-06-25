using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.AspNetCore.IntegrationTests.Utils.Abstractions;
using Entra.EventHandlers.Builders;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Utils;

public class TestAttributeCollectionSubmitHandler : TestHandlerBase, IAttributeCollectionSubmitHandler
{
    public Task<AttributeCollectionSubmitResponse> HandleAsync(
        AttributeCollectionSubmitEvent request,
        CancellationToken cancellationToken = default)
    {
        WasCalled = true;
        CapturedCancellationToken = cancellationToken;

        return Task.FromResult(
            EntraEventResponses
                .AttributeCollectionSubmit()
                .ContinueWithDefaultBehavior()
                .Build());
    }
}
