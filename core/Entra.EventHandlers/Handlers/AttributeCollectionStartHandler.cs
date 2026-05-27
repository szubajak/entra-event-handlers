using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.ResponseBuilders;
using Mediator;

namespace Entra.EventHandlers.Handlers;

public class AttributeCollectionStartHandler : IRequestHandler<AttributeCollectionStartEvent, AttributeCollectionStartResponse>
{
    public async ValueTask<AttributeCollectionStartResponse> Handle(AttributeCollectionStartEvent request, CancellationToken cancellationToken)
    {
        // Simulate some async work
        await Task.Delay(1, cancellationToken);

        // For this example, we will just return a response that continues with the default behavior.
        return EntraEventResponses
            .AttributeCollectionStart()
            .ContinueWithDefaultBehavior()
            .Build();
    }
}
