using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Handlers.Base;
using Entra.EventHandlers.TestHelpers;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.UnitTests.Utils.Handlers;

public class TestAttributeCollectionStartHandler(ILogger logger)
    : AttributeCollectionStartHandlerBase(logger)
{
    public HandlerCoreTest CoreTest { get; } = new HandlerCoreTest();

    public AttributeCollectionStartResponse ResponseToReturn { get; set; } = new AttributeCollectionStartResponse
    { 
        Data = new AttributeCollectionStartResponsePayload()
    };

    protected override Task<AttributeCollectionStartResponse> HandleCoreAsync(
        AttributeCollectionStartEvent request,
        CancellationToken cancellationToken)
    {
        CoreTest.Record(cancellationToken);
        return Task.FromResult(ResponseToReturn);
    }
}
