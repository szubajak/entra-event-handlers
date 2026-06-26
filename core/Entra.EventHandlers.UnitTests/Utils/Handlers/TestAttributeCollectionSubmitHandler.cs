using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Handlers.Base;
using Entra.EventHandlers.TestHelpers;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.UnitTests.Utils.Handlers;

public class TestAttributeCollectionSubmitHandler(ILogger logger)
    : AttributeCollectionSubmitHandlerBase(logger)
{
    public HandlerCoreTest CoreTest { get; } = new HandlerCoreTest();

    public AttributeCollectionSubmitResponse ResponseToReturn { get; set; } = new AttributeCollectionSubmitResponse
    { 
        Data = new AttributeCollectionSubmitResponsePayload()
    };

    protected override Task<AttributeCollectionSubmitResponse> HandleCoreAsync(
        AttributeCollectionSubmitEvent request,
        CancellationToken cancellationToken)
    {
        CoreTest.Record(cancellationToken);
        return Task.FromResult(ResponseToReturn);
    }
}
