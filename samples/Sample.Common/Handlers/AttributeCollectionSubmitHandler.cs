using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Builders;
using Entra.EventHandlers.Handlers.Base;
using Microsoft.Extensions.Logging;

namespace Sample.Common.Handlers;

public class AttributeCollectionSubmitHandler(ILogger<AttributeCollectionSubmitHandler> logger)
    : AttributeCollectionSubmitHandlerBase(logger)
{
    protected override Task<AttributeCollectionSubmitResponse> HandleCoreAsync(
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