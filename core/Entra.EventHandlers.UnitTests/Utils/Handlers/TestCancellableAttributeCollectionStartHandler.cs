using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Handlers.Base;
using Entra.EventHandlers.TestHelpers;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.UnitTests.Utils.Handlers;

public class TestCancellableAttributeCollectionStartHandler(ILogger logger)
    : AttributeCollectionStartHandlerBase(logger)
{
    public HandlerCoreTest CoreTest { get; } = new HandlerCoreTest();

    protected override Task<AttributeCollectionStartResponse> HandleCoreAsync(
        AttributeCollectionStartEvent request,
        CancellationToken cancellationToken = default)
    {
        CoreTest.Record(cancellationToken);
        // Return a canceled task to simulate cooperative cancellation path
        return Task.FromCanceled<AttributeCollectionStartResponse>(cancellationToken);
    }
}
