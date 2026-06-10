using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Handlers.Base;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.UnitTests.Utils.Handlers;

public class TestAttributeCollectionSubmitHandler(ILogger logger)
    : AttributeCollectionSubmitHandlerBase(logger)
{
    public HandlerCoreTest CoreTest { get; } = new HandlerCoreTest();

    public AttributeCollectionSubmitResponse ResponseToReturn { get; set; } = new();

    protected override Task<AttributeCollectionSubmitResponse> HandleCore(
        AttributeCollectionSubmitEvent request,
        CancellationToken cancellationToken)
    {
        CoreTest.Record(cancellationToken);
        return Task.FromResult(ResponseToReturn);
    }
}
