using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Abstractions.Results;
using Entra.EventHandlers.Builders;
using Entra.EventHandlers.TestHelpers;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Utils;

public class TestAttributeCollectionSubmitHandler : TestHandlerBase, IAttributeCollectionSubmitHandler
{
    public Task<EntraHandlerResult<AttributeCollectionSubmitResponse>> HandleAsync(
        AttributeCollectionSubmitEvent request,
        CancellationToken cancellationToken = default)
    {
        request.Validate();

        WasCalled = true;
        CapturedCancellationToken = cancellationToken;

        var response = EntraEventResponses.AttributeCollectionSubmit()
            .ContinueWithDefaultBehavior()
            .Build();

        return Task.FromResult(new EntraHandlerResult<AttributeCollectionSubmitResponse>(response));
    }
}
