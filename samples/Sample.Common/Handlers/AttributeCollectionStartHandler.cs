using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Builders;
using Entra.EventHandlers.Handlers.Base;
using Microsoft.Extensions.Logging;

namespace Sample.Common.Handlers;

public class AttributeCollectionStartHandler(ILogger<AttributeCollectionStartHandler> logger)
    : AttributeCollectionStartHandlerBase(logger)
{
    protected override Task<AttributeCollectionStartResponse> HandleCore(
        AttributeCollectionStartEvent request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(
            EntraEventResponses
                .AttributeCollectionStart()
                .ContinueWithDefaultBehavior()
                .Build());
    }
}