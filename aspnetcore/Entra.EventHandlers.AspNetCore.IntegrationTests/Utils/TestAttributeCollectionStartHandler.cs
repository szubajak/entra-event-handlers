using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Abstractions.Results;
using Entra.EventHandlers.Builders;
using Entra.EventHandlers.TestHelpers;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Utils;

public class TestAttributeCollectionStartHandler : TestHandlerBase, IAttributeCollectionStartHandler
{
    public Task<EntraHandlerResult<AttributeCollectionStartResponse>> HandleAsync(
        AttributeCollectionStartEvent request,
        CancellationToken cancellationToken = default)
    {
        request.Validate();

        WasCalled = true;
        CapturedCancellationToken = cancellationToken;

        var response = EntraEventResponses.AttributeCollectionStart()
            .ContinueWithDefaultBehavior()
            .Build();

        return Task.FromResult(new EntraHandlerResult<AttributeCollectionStartResponse>(response));
    }
}
