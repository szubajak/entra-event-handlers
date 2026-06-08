using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Builders;
using Entra.EventHandlers.Handlers.Base;

namespace ApiSample.Handlers;

public class AttributeCollectionSubmitHandler(ILogger<AttributeCollectionSubmitHandler> logger)
    : AttributeCollectionSubmitHandlerBase(logger)
{
    protected override Task<AttributeCollectionSubmitResponse> HandleCore(
        AttributeCollectionSubmitEvent request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(
            EntraEventResponses
                .AttributeCollectionSubmit()
                .ContinueWithDefaultBehavior()
                .Build());
    }
}