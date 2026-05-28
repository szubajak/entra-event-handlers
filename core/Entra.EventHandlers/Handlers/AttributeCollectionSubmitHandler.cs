using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.ResponseBuilders;

namespace Entra.EventHandlers.Handlers;

public class AttributeCollectionSubmitHandler : IAttributeCollectionSubmitHandler
{
    public async Task<AttributeCollectionSubmitResponse> Handle(AttributeCollectionSubmitEvent request, CancellationToken cancellationToken)
    {
        // Simulate some async work
        await Task.Delay(1, cancellationToken);

        // For this example, we will just return a response that continues with the default behavior.
        return EntraEventResponses
            .AttributeCollectionSubmit()
            .ContinueWithDefaultBehavior()
            .Build();
    }
}
