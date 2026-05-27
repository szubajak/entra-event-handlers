using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.ResponseBuilders;
using Mediator;

namespace Entra.EventHandlers.Handlers;

public class AttributeCollectionSubmitHandler : IRequestHandler<AttributeCollectionSubmitEvent, AttributeCollectionSubmitResponse>
{
    public async ValueTask<AttributeCollectionSubmitResponse> Handle(AttributeCollectionSubmitEvent request, CancellationToken cancellationToken)
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
